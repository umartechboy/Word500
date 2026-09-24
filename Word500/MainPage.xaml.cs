namespace Word500
{
    public partial class MainPage : ContentPage
    {
        public MainPage() : this(8)
        {
        }
        public MainPage(int wordCount)
        {
            InitializeComponent();
            keyboard.KeyPressed += (s, e) =>
            {
                if (e.Key == "space")
                    wordBoard.appendLetter('_');
                else if (e.Key == "backspace")
                    wordBoard.appendLetter('\b');
                else if (e.Key == "reset")
                {
                }
                else if (e.Key == "hard reset")
                {
                }
                else if (e.Key == "submit")
                {
                }
                else if (e.Key.Length == 1)
                    wordBoard.appendLetter(e.Key[0]);
            };
        }
    }
}
