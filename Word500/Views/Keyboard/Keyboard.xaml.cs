using static Word500.FunctionKey;

namespace Word500;

public partial class Keyboard : ContentView
{
    public class KeyPressedEventArgs : EventArgs { public string Key { get; set; } public KeyFunction Function { get; set; } = KeyFunction.None; }
    public delegate void KeyPressedHandler(object sender, KeyPressedEventArgs e);
    public event KeyPressedHandler KeyPressed;
	KeyboardRow [] rows;
    public Keyboard()
	{
        InitializeComponent();
        var row1 = new KeyboardRow("QWERTYUIOP");
		var row2 = new KeyboardRow("\tASDFGHJKL\t");
		var row3 = new KeyboardRow("\t\tZXCVBNM\t\t");
		var row4 = new KeyboardRow("\t\t\t \t\t\t");
		row1.KeyPressed += (s, e) => KeyPressed?.Invoke(s, e);
		row2.KeyPressed += (s, e) => KeyPressed?.Invoke(s, e);
        row3.KeyPressed += (s, e) => KeyPressed?.Invoke(s, e);
        row4.KeyPressed += (s, e) => KeyPressed?.Invoke(s, e);
        rows = new KeyboardRow[] { row1, row2, row3, row4};

        lRowGrid.Children.Add(row1);
		lRowGrid.Children.Add(row2);
		lRowGrid.Children.Add(row3);
        lRowGrid.Children.Add(row4);

		bBackspace.KeyPressed += (s,e) => KeyPressed?.Invoke(s, e);
		bSubmit.KeyPressed += (s, e) => KeyPressed?.Invoke(s, e);
		bClear.KeyPressed += (s, e) => KeyPressed?.Invoke(s, e);
    }
	public KeyboardKey this[char chr]
	{
		get
		{
			foreach(var row in rows)
			{
				if (row[chr] != null)
					return row[chr];
			}
			return null;
		}
	}
}