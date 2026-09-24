using static Word500.Keyboard;
using static Word500.LetterTile;

namespace Word500;

public partial class KeyboardKey : ContentView
{
    public string Character { get; private set; } = "";
	public KeyboardKey(string chr)
	{
		InitializeComponent();
        if (chr == "\r")
            chr = "reset";
        else if (chr == "\n")
            chr = "hard reset";
        else if (chr == "\b")
            chr = "backspace";
        text.Text = chr;
        if (chr == "\t")
        {
            tileBorder.Stroke = null;
        }
        Character = chr;
    }
    public event KeyPressedHandler KeyPressed;
    protected override void OnHandlerChanged()
    {
        void handler(object s, object e)
        {
            if (this.Window == null)
                return;
            this.WidthRequest = this.Window.Width / 13;
            this.Window.SizeChanged += (s, e) => handler(s, e);
            if (this.Character == "\t")
                this.WidthRequest /= 2;
            this.HeightRequest = this.WidthRequest;
            if (this.Character == " ")
                this.WidthRequest *= 5;
        }
        handler(null, null);
    }
    public KeyboardKey():this(" ")
    {

    }

    private DateTime pressStart;
    private static readonly TimeSpan HoldThreshold = TimeSpan.FromSeconds(0.7);
    bool _isused = false;
    public bool IsUsed
    {
        get => _isused; set
        {
            if (Character != "" && Character != " " && Character != "reset" && Character != "hard reset" && Character != "backspace")
            {
                _isused = value;
                tileBorder.Opacity = value ? 0.3 : 1.0;
            }
        }
    }
    bool hasReleased = true;
    private void PointerGestureRecognizer_PointerPressed(object sender, PointerEventArgs e)
    {
        if (Character == "\t")
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
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    IsUsed = !IsUsed;
                    await tileBorder.ScaleToAsync(1.1, 200);
                    await tileBorder.ScaleToAsync(1, 100);
                });
            }
        }).Start();
    }

    private async void PointerGestureRecognizer_PointerReleased(object sender, PointerEventArgs e)
    {
        if(Character == "\t") return;
        tileBorder.CancelAnimations();
        hasReleased = true;
        var elapsed = DateTime.UtcNow - pressStart;

        if (elapsed < HoldThreshold)
        {
            KeyPressed?.Invoke(this, new KeyPressedEventArgs() { Key = this.Character });
            await tileBorder.ScaleToAsync(1.1, 100);
            await tileBorder.ScaleToAsync(1, 100);
        }
        else
        {
            // handled in timer
        }
    }
}