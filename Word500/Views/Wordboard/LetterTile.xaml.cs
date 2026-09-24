namespace Word500;

public partial class LetterTile : ContentView
{
	public LetterTile()
	{
		InitializeComponent();
	}
	private MarkStates markState;
	public MarkStates TestState
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
	public event EventHandler? TestStateChanged;
	public event EventHandler? Locked;

	private DateTime pressStart;
	private static readonly TimeSpan HoldThreshold = TimeSpan.FromSeconds(0.7);
	bool hasReleased = true;

	private void OnPointerPressed(object? sender, PointerEventArgs e)
	{
		if (DisableClick) return;
		if (string.IsNullOrEmpty(label.Text))
			return;
		hasReleased = false;
        pressStart = DateTime.UtcNow;
		tileBorder.CancelAnimations();
		tileBorder.ScaleToAsync(0.7, (uint)HoldThreshold.TotalMilliseconds);
		new Task(async () =>
		{
			await Task.Delay((int)HoldThreshold.TotalMilliseconds);

            if (hasReleased)
                return;
			var elapsed = DateTime.UtcNow - pressStart;
            if (elapsed.TotalMilliseconds > HoldThreshold.TotalMilliseconds - 50) // it was a hold
			{
                await tileBorder.ScaleToAsync(1.1, 200);
                await tileBorder.ScaleToAsync(1, 100);
                Locked?.Invoke(this, EventArgs.Empty);

            }
		}).Start();
	}

	private async void OnPointerReleased(object? sender, PointerEventArgs e)
	{
		if (DisableClick) return;
        if (string.IsNullOrEmpty(label.Text))
            return;
        tileBorder.CancelAnimations();
		var elapsed = DateTime.UtcNow - pressStart;
		hasReleased = true;

        if (elapsed < HoldThreshold)
        {
            TestState++;
            if (TestState > MarkStates.Green)
                TestState = MarkStates.Null;
            await tileBorder.ScaleToAsync(1.1, 100);
            await tileBorder.ScaleToAsync(1, 100);
			TestStateChanged?.Invoke(this, EventArgs.Empty);
		}
		else
		{
			// handled in timer
		}
	}

	internal void shakeNo()
	{
        tileBorder.TranslateToAsync(5, 0, 50).ContinueWith((b) =>
        {
            tileBorder.TranslateToAsync(-5, 0, 50).ContinueWith((b) =>
            {
                tileBorder.TranslateToAsync(5, 0, 50).ContinueWith((b) =>
                {
                    tileBorder.TranslateToAsync(-5, 0, 50).ContinueWith((b) =>
                    {
                        tileBorder.TranslateToAsync(0, 0, 50);
                    });
                });
            });
        });
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