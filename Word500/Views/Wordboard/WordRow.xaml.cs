namespace Word500;
using System.Linq;

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
        scoreTiles = new LetterTile[3];
        for (int i = 0; i < scoreTiles.Length; i++)
        {
            scoreTiles[i] = new LetterTile { DisableClick = true, HorizontalOptions = LayoutOptions.Fill };
            scoreTiles[i].SizeChanged += OnTileSizeChanged;
            scoreTiles[i].Margin = new Thickness(this.Margin.Bottom, 0, 0, 0);
            scoreTiles[i].MarkState = (LetterTile.MarkStates)(3 - i);
            tileGrid.Add(scoreTiles[i], wordCount + i, 0);
        }
    }

    public string WordEntered { get { return string.Join("", letterTiles.Select(t => t.Label)); } }

    public void setLetter(char chr, int currentLetter)
    {
        letterTiles[currentLetter].Label = chr.ToString();
    }
    public void shakeNo()
    {
        foreach (var letterTile in letterTiles) 
        {
            letterTile.shakeNo();
        }
    }
    private void OnTileSizeChanged(object? sender, EventArgs e)
    {
        var tile = (LetterTile)sender!;
        if (tile.Width > 0)
            tile.HeightRequest = tile.Width;
    }
}
