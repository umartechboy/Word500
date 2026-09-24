using static Word500.Keyboard;
using static Word500.KeyboardKey;

namespace Word500;

public partial class KeyboardRow : ContentView
{
    public event KeyPressedHandler KeyPressed;
    public KeyboardRow(string keys)
    {
        InitializeComponent();
        for (int i = 0; i < keys.Length; i++)
        {
            var keyView = new KeyboardKey(keys[i].ToString()) { HorizontalOptions = LayoutOptions.Fill };
            keyView.KeyPressed += (s, e) => KeyPressed?.Invoke(s, e);
            row.Children.Add(keyView);
        }
    }
    public KeyboardRow():this(" ")
	{
	}
}