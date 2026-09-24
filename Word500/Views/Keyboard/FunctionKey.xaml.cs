using static Word500.Keyboard;
using static Word500.LetterTile;

namespace Word500;

public partial class FunctionKey : ContentView
{
    Border tileBorder;
    Image icon;

    KeyFunction _function;
    public KeyFunction Function
    {
        get => _function; set
        {
            _function = value;
            if (icon == null) return;
            if (value == KeyFunction.Submit)
                icon.Source = "submit_icon.png";
            if (value == KeyFunction.BackSpace)
                icon.Source = "backspace_icon.png";
            if (value == KeyFunction.Clear)
                icon.Source = "clear.png";
        }
    }

    public enum KeyFunction
    {
        Clear,
        Submit,
        BackSpace,
        None,
        HardClear,
        BackSpaceFullLine
    }
    public FunctionKey()
    {
        InitializeComponent();
        tileBorder = (Border)Content;
        icon = (Image)tileBorder.Content;
        Function = _function;
    }
    public event KeyPressedHandler KeyPressed;
    protected override void OnHandlerChanged()
    {
        void handler(object s, object e)
        { 
            this.HeightRequest = this.Window.Width / 13;
        }
        handler(null, null);
    }
    private DateTime pressStart;
    private static readonly TimeSpan HoldThreshold = TimeSpan.FromSeconds(0.7);
    bool hasReleased = true;
    private void PointerGestureRecognizer_PointerPressed(object sender, PointerEventArgs e)
    {
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
                    // hard clear
                    await tileBorder.ScaleToAsync(1.1, 200);
                    await tileBorder.ScaleToAsync(1, 100);
                });
            }
        }).Start();
    }

    private async void PointerGestureRecognizer_PointerReleased(object sender, PointerEventArgs e)
    {
        tileBorder.CancelAnimations();
        hasReleased = true;
        var elapsed = DateTime.UtcNow - pressStart;

        if (elapsed < HoldThreshold)
        {
            await tileBorder.ScaleToAsync(1.1, 100);
            await tileBorder.ScaleToAsync(1, 100);
            KeyPressed?.Invoke(this, new KeyPressedEventArgs() { Function = this.Function });
        }
        else
        {
            // handled in timer
        }
    }
}