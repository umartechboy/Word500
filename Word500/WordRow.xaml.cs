namespace Word500;

public partial class WordRow : ContentView
{
    public LetterTile[] letterTiles;
    LetterTile[] scoreTiles;
    public WordRow():this(5)
	{
	}
    public WordRow(int wordCount)
    {
        this.Margin = new Thickness(0, 0, 0, 3);
        InitializeComponent();
        letterTiles = new LetterTile[wordCount];
        scoreTiles = new LetterTile[3];
        int totalTiles = wordCount + 3;

        for (int i = 0; i < totalTiles; i++)
            tileGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

        for (int i = 0; i < letterTiles.Length; i++)
        {
            letterTiles[i] = new LetterTile { HorizontalOptions = LayoutOptions.Fill };
            letterTiles[i].SizeChanged += OnTileSizeChanged;
            letterTiles[i].Margin = new Thickness(0, 0, this.Margin.Bottom, 0);
            tileGrid.Add(letterTiles[i], i, 0);
        }
        for (int i = 0; i < scoreTiles.Length; i++)
        {
            scoreTiles[i] = new LetterTile { DisableClick = true, HorizontalOptions = LayoutOptions.Fill };
            scoreTiles[i].SizeChanged += OnTileSizeChanged;
            scoreTiles[i].Margin = new Thickness(this.Margin.Bottom, 0, 0, 0);
            tileGrid.Add(scoreTiles[i], wordCount + i, 0);
        }
    }

    int currentLetter = 0;
    public void appendLetter(char chr)
    {
        if (chr == '\b')
        {
            if (currentLetter <= 0)
                return;
            currentLetter--;
            letterTiles[currentLetter].Label = "";
            return;
        }
        if (currentLetter >= letterTiles.Length)
        {
            return;
        }
        letterTiles[currentLetter].Label = chr.ToString();
        currentLetter++;
    }
    private void OnTileSizeChanged(object? sender, EventArgs e)
    {
        var tile = (LetterTile)sender!;
        if (tile.Width > 0)
            tile.HeightRequest = tile.Width;
    }
}
