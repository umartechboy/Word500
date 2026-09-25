using System.Runtime.Serialization;
namespace Word500;

public partial class WordGrid : ContentView
{
    int _retriesCount = 0;
    int _wordLength = 0;
    public int WordLength
    {
        get => _wordLength;
        set
        {
            if (_wordLength == value)
                return;
            _wordLength = value;
            //resetViews();
        }
    }
    public int RetriesCount
    {
        get => _retriesCount;
        set
        {
            if (_retriesCount == value)
                return;
            _retriesCount = value;
            //resetViews();
        }

    }

    string _word;
    public string Word { get => _word; set { _word = value.ToUpper(); WordLength = value.Length; } }
    public string UserWord { get { return wordRows[currentRow].WordEntered; } }
    public event EventHandler TestStateChanged;
    public void ResetViews()
    {
        wordRows = new WordRow[RetriesCount];
        lWordGrid.Children.Clear();
        for (int i = 0; i < wordRows.Length; i++)
        {
            wordRows[i] = new WordRow(WordLength);
            wordRows[i].TestStateChanged += (s, e) => TestStateChanged?.Invoke(s, e);
            lWordGrid.Children.Add(wordRows[i]);
        }
    }
    WordRow[] wordRows;
    public WordGrid(int wordCount)
    {
        InitializeComponent();
        wordRows = new WordRow[wordCount];
        for (int i = 0; i < wordRows.Length; i++)
        {
            wordRows[i] = new WordRow();
            lWordGrid.Children.Add(wordRows[i]);
        }
    }
    double lastCalcOn = -1;
    protected override void OnHandlerChanged()
    {
        if (this.Window == null)
            return;
        Window.SizeChanged += sizeChanged;
        void sizeChanged (object s, EventArgs  e)
        {
            if (lastCalcOn == this.Window.Width)
                return;
            lastCalcOn = this.Window.Width;
            Window.SizeChanged -= sizeChanged;
            var margin = 20;
            var sp = 8;
            var screenUsage = 0.7;
            double sizeByWidth = (this.Window.Width - margin * 2) / (Word.Length + 3) - sp;
            double sizeByHeight = (this.Window.Height - margin * 2) * screenUsage / wordRows.Length - sp;
            double size = Math.Min(sizeByWidth, sizeByHeight);

            foreach (var row in wordRows)
                row.TileSize = size;
        };
        sizeChanged(null, null);
    }
    public WordGrid() : this(8)
    {
    }
    int currentRow = 0;
    int currentLetter = 0;
    string lastWordForSquible = null;
    public void appendLetter(char chr)
    {
        lastWordForSquible = null;
        if (currentRow >= wordRows.Length)
            return;
        //if (UserWord.Contains(chr)) 
        //{
        //    wordRows[currentRow].shakeNo(UserWord.IndexOf(chr));
        //    return;
        //}
        if (chr == '\b')
        {
            if (currentLetter <= 0)
                return;
            currentLetter--;
            wordRows[currentRow].setLetter('\0', currentLetter);
            return;
        }
        if (currentLetter >= WordLength)
        {
            return;
        }
        wordRows[currentRow].setLetter(chr, currentLetter);
        currentLetter++;
    }

    static int countCorrect(string user, string correct)
    {
        int c = 0;
        for (int i = 0; i < user.Length; i++)
        {
            if (user[i] == correct[i])
                c++;
        }
        return c;
    }
    static int countPresent(string user, string correct)
    {
        int c = 0;
        for (int i = 0; i < user.Length; i++)
        {
            if (correct.Contains(user[i]) && correct[i] != user[i])
                c++;
        }
        return c;
    }
    static int countWrong(string user, string correct)
    {
        return correct.Length - countCorrect(user, correct) - countPresent(user, correct);
    }
    internal void EvaluateCurrent(bool ignoreSpelling = false)
    {
        for (int i = 0; i < currentRow; i++)
        {
            if (wordRows[i].WordEntered == wordRows[currentRow].WordEntered) // cant waste the word again
            {
                wordRows[i].shakeNo();
                wordRows[currentRow].shakeNo();
                return;
            }
        }
        if (currentLetter < WordLength - 1 || (!Dictionary.GetWords().Contains(UserWord) && !ignoreSpelling))
        {
            wordRows[currentRow].shakeNo();
            return;
        }
        else
        { // we have a complete dictionary word. 
            wordRows[currentRow].MarkEvaluation(countCorrect(UserWord, Word), countPresent(UserWord, Word), countWrong(UserWord, Word));
        }
        if (countWrong(UserWord, Word) == Word.Length)
        {
            foreach (var c in UserWord)
                MarkTestState(c.ToString(), LetterTile.MarkStates.Red);
        }
        currentRow++;
        currentLetter = 0;
    }

    internal void MarkTestState(string label, LetterTile.MarkStates testState)
    {
        for (int i = 0; i <= currentRow; i++)
        {
            wordRows[i].MarkTestState(label, testState);
        }
    }

    internal void HighlightWrong()
    {
        foreach(var row in wordRows)
        {
            if (row.AllWrong)
            {
                foreach(var c in row.WordEntered)
                {
                    MarkTestState(c.ToString(), LetterTile.MarkStates.Red);
                }
            }
        }
    }

    internal void ClearAllTests()
    {
        foreach (var row in wordRows)
        {
            foreach (var c in row.WordEntered)
            {
                MarkTestState(c.ToString(), LetterTile.MarkStates.Null);
            }
        }
    }

    internal void squibleCurrent(bool hard)
    {
        string UserWord = this.UserWord;
        if (lastWordForSquible != null)
            UserWord = lastWordForSquible;
        else
            lastWordForSquible = this.UserWord;
        var newWord = Dictionary.Squible(UserWord, Word, hard);
        if (newWord == UserWord)
            wordRows[currentRow].shakeNo();
        else
        {
            wordRows[currentRow].WordEntered = newWord;
            currentLetter = newWord.Length;
        }
    }

    public async Task CollapseRemaining()
    {
        for (int i = 1; i< wordRows.Length; i++)
        {
            await wordRows[i].CollapseRemaining();
        }
    }
    public async Task ScaleUpAll()
    {
        for (int i = 0; i < wordRows.Length; i++)
        {
            await wordRows[i].ScaleUpAll();
        }
    }
}
