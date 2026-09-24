namespace Word500;

public partial class WordGrid : ContentView
{
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