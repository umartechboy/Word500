namespace Word500;

public partial class Keyboard : ContentView
{
	public event Word500.KeyboardKey.OnPressed KeyPressed;
	public Keyboard()
	{
		InitializeComponent();
		var row1 = new KeyboardRow("QWERTYUIOP");
		var row2 = new KeyboardRow("\tASDFGHJKL\t");
		var row3 = new KeyboardRow("\t\tZXCVBNM\t\t");
		var row4 = new KeyboardRow("\r \n\b");
		row1.KeyPressed += (s, e) => KeyPressed?.Invoke(s, e);
		row2.KeyPressed += (s, e) => KeyPressed?.Invoke(s, e);
        row3.KeyPressed += (s, e) => KeyPressed?.Invoke(s, e);
        row4.KeyPressed += (s, e) => KeyPressed?.Invoke(s, e);

        lRowGrid.Children.Add(row1);
		lRowGrid.Children.Add(row2);
		lRowGrid.Children.Add(row3);
        lRowGrid.Children.Add(row4);

    }
}