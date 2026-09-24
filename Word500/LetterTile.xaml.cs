namespace Word500;

public partial class LetterTile : ContentView
{
	public LetterTile()
	{
		InitializeComponent();
	}
	private MarkStates markState;
	public MarkStates MarkState
	{
		get => markState;
		set
		{
			markState = value;
			tileBorder.BackgroundColor = value switch
			{
				MarkStates.Red => Colors.Red,
				MarkStates.Yellow => Colors.Gold,
				MarkStates.Green => Colors.Green,
				_ => Colors.Transparent
			};
		}
	}
	private void OnTileTapped(object? sender, TappedEventArgs e)
	{
		if (!DisableClick)
		{
			MarkState++;
			if (MarkState > MarkStates.Green)
				MarkState = MarkStates.Null;
		}
	}
	public double Size
	{
		get { return tileBorder.WidthRequest; }
		set { tileBorder.HeightRequest = tileBorder.WidthRequest = value > 5 ? value : 5; }
	}
	public bool DisableClick { get; set; } = false;
	public string Label { get { return label.Text; } set { label.Text = value; } }
	public enum MarkStates : int
	{
		Null = 0,
		Red,
		Yellow,
		Green
	}
}