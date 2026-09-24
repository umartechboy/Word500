namespace Word500
{
    public partial class MainPage : ContentPage
    {
        WordRow[] wordRows;
        public MainPage() : this(8)
        {
        }
public MainPage(int wordCount)
        {
            InitializeComponent();
            wordRows = new WordRow[wordCount];
            for (int i = 0; i < wordRows.Length; i++)
            {
                wordRows[i] = new WordRow();
                lWordGrid.Children.Add(wordRows[i]);
            }
        }
    }
}
