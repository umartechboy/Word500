namespace Word500
{
    [QueryProperty(nameof(Letters), "letters")]
    [QueryProperty(nameof(Chances), "chances")]
    public partial class GamePage : ContentPage
    {
        private int _letters = 5;
        private int _chances = 8;

        public string Letters
        {
            set { if (int.TryParse(value, out var v)) _letters = v; ApplySettings(); }
        }

        public string Chances
        {
            set { if (int.TryParse(value, out var v)) _chances = v; ApplySettings(); }
        }

        private void ApplySettings()
        {
            wordBoard.WordLength = _letters;
            wordBoard.RetriesCount = _chances;
        }

        public GamePage()
        {
            InitializeComponent();
            wordBoard.TestStateChanged += WordBoard_TestStateChanged;
            keyboard.KeyPressed += (s, e) =>
            {
                if (e.Key == "space")
                    wordBoard.appendLetter('_');
                else if (e.Function == FunctionKey.KeyFunction.BackSpace)
                    wordBoard.appendLetter('\b');
                else if (e.Function == FunctionKey.KeyFunction.BackSpaceFullLine)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        for (int i = 0; i < wordBoard.WordLength; i++)
                            wordBoard.appendLetter('\b');
                    });
                }
                else if (e.Function == FunctionKey.KeyFunction.Clear)
                {
                    wordBoard.HighlightWrong();
                }
                else if (e.Function == FunctionKey.KeyFunction.Squible)
                {
                    wordBoard.squibleCurrent(false);
                }
                else if (e.Function == FunctionKey.KeyFunction.HardSquible)
                {

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        wordBoard.squibleCurrent(true);
                    });
                }
                else if (e.Function == FunctionKey.KeyFunction.HardClear)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        wordBoard.ClearAllTests();
                    });
                }
                else if (e.Function == FunctionKey.KeyFunction.Submit)
                {
                    wordBoard.EvaluateCurrent();
                }
                else if (e.Function == FunctionKey.KeyFunction.ForceSubmit)
                {
                    // This needs to be invoked in the UI thread
                    MainThread.BeginInvokeOnMainThread(() => { wordBoard.EvaluateCurrent(true); });

                }
                else if (e.Key.Length == 1)
                {
                    wordBoard.appendLetter(e.Key[0]);
                    keyboard[e.Key[0]].IsUsed = true;
                }


            };
        }

        private void WordBoard_TestStateChanged(object? sender, EventArgs e)
        {
        }
    }
}
