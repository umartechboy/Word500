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
                else if (e.Function == FunctionKey.KeyFunction.BackSpace)
                    wordBoard.appendLetter('\b');
                else if (e.Function == FunctionKey.KeyFunction.BackSpaceFullLine)
                {
                    for (int i = 0; i < wordBoard.WordLength; i++)
                        wordBoard.appendLetter('\b');
                }
                else if (e.Function == FunctionKey.KeyFunction.Clear)
                {
                }
                else if (e.Function == FunctionKey.KeyFunction.HardClear)
                {
                }
                else if (e.Function == FunctionKey.KeyFunction.Submit)
                {
                }
                else if (e.Key.Length == 1)
                    wordBoard.appendLetter(e.Key[0]);
            };
        }
    }
}
