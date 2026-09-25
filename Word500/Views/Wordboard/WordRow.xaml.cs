namespace Word500;
using System.Linq;

public partial class WordRow : ContentView
{
    public LetterTile[] letterTiles;
    LetterTile[] scoreTiles;
    public event EventHandler TestStateChanged;
    public bool AllWrong
    {
        get =>
            scoreTiles[2].Label == letterTiles.Length.ToString();
    }
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
            letterTiles[i].Margin = new Thickness(0, 0, this.Margin.Bottom, 0);
            letterTiles[i].TestStateChanged += (s, e) => TestStateChanged?.Invoke(s, e);
            letterTiles[i].Scale = 0;
            letterTiles[i].Opacity = 0;
            tileGrid.Add(letterTiles[i], i, 0);
        }
        scoreTiles = new LetterTile[3];
        for (int i = 0; i < scoreTiles.Length; i++)
        {
            scoreTiles[i] = new LetterTile { DisableClick = true, HorizontalOptions = LayoutOptions.Fill };
            scoreTiles[i].Margin = new Thickness(this.Margin.Bottom, 0, 0, 0);
            scoreTiles[i].TestState = (LetterTile.MarkStates)(3 - i);
            scoreTiles[i].Scale = 0;
            scoreTiles[i].Opacity = 0;
            tileGrid.Add(scoreTiles[i], wordCount + i, 0);
        }
    }

    public string WordEntered
    {
        get { return string.Join("", letterTiles.Select(t => t.Label)); }
        set
        {
            for (int i = 0; i < letterTiles.Length; i++)
            {
                if (i < value.Length)
                    letterTiles[i].Label = value[i].ToString();
                else
                    letterTiles[i].Label = "";
            }
        }
    }

    double _tileSize = -1;
    public double TileSize
    {
        set
        {
            foreach (var tile in letterTiles)
                tile.WidthRequest = tile.HeightRequest = value;
            foreach (var tile in scoreTiles)
                tile.WidthRequest = tile.HeightRequest = value;
            _tileSize = value;
        }
        get => _tileSize;
    }

    public void setLetter(char chr, int currentLetter)
    {
        letterTiles[currentLetter].Label = chr.ToString().Replace("\0", "").Replace(" ", "_");
    }
    public void shakeNo(int i = -1)
    {
        if (i < 0)
            foreach (var letterTile in letterTiles) 
            {
                letterTile.shakeNo();
            }
        else
        {
            letterTiles[i].shakeNo();
        }
    }

    internal void MarkEvaluation(int correct, int present, int wrong)
    {
        scoreTiles[0].Label = correct.ToString();
        scoreTiles[1].Label = present.ToString();
        scoreTiles[2].Label = wrong.ToString();
    }

    internal void MarkTestState(string label, LetterTile.MarkStates testState)
    {
        foreach(var tile in letterTiles)
        {
            if (tile.Label == label)
                tile.TestState = testState;
        }
    }

    public async Task CollapseRemaining()
    {
        var allTiles = new List<LetterTile>();
        allTiles.AddRange(letterTiles);
        allTiles.AddRange(scoreTiles);
        foreach(var tile in allTiles)
            await tile.FadeToAsync(0.1, 30);
    }

    public async Task ScaleUpAll()
    {
        var allTiles = new List<LetterTile>();
        allTiles.AddRange(letterTiles);
        allTiles.AddRange(scoreTiles);
        foreach (var tile in allTiles)
        {
            tile.FadeToAsync(1, 300);
            tile.ScaleToAsync(1.2, 300);
            tile.ScaleToAsync(1, 100);
            await Task.Delay(30);
        }
    }

    public async Task Jump()
    {
        var allTiles = new List<LetterTile>();
        allTiles.AddRange(letterTiles);
        allTiles.AddRange(scoreTiles);
        foreach (var tile in allTiles)
        {
            tile.TranslateToAsync(0, 10, 50).ContinueWith((a) => tile.TranslateToAsync(0, -10, 120).ContinueWith((b) => tile.TranslateToAsync(0, 0, 50)));
            await Task.Delay(30);
        }
    }
}
