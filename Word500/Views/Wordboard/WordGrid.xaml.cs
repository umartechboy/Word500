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
    public string Word { get => _word; set { _word = value; WordLength = value.Length; } }
    public string UserWord { get { return wordRows[currentRow].WordEntered; } }

    void resetViews()
    {
        wordRows = new WordRow[RetriesCount];
        lWordGrid.Children.Clear();
        for (int i = 0; i < wordRows.Length; i++)
        {
            wordRows[i] = new WordRow(WordLength);
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
	public WordGrid():this(8)
	{
    }
    int currentRow = 0;
    int currentLetter = 0;
    public void appendLetter(char chr)
    {
        if (chr == '\b')
        {
            if (currentLetter <= 0)
                return;
            currentLetter--;
            wordRows[currentRow].setLetter(chr, currentLetter);
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
    internal void EvaluateCurrent()
    {
        if (currentLetter < WordLength - 1) 
        {
            wordRows[currentRow].shakeNo();
        }
    }
}