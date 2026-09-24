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
    public void appendLetter(char chr)
    {
        wordRows[currentRow].appendLetter(chr);
    }
}