using System.Runtime.Serialization;

namespace Word500;

public partial class WordGrid : ContentView
{
    int _retriesCount = 8;
    int _wordLength = 5;
    public int WordLength
    {
        get => _wordLength;
        set
        {
            _wordLength = value;
            resetViews();
        }
    }
    public int RetriesCount
    {
        get => _retriesCount;
        set
        {
            _retriesCount = value;
            resetViews();
        }

    }

    string _word;
    public string Word { get => _word; set { _word = value.ToUpper(); WordLength = value.Length; } }
    public string UserWord { get { return wordRows[currentRow].WordEntered; } }
    public event EventHandler TestStateChanged;
    void resetViews()
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
    public WordGrid() : this(8)
    {
    }
    int currentRow = 0;
    int currentLetter = 0;
    public void appendLetter(char chr)
    {
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
}
