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
            // var words = Dictionary.GetWords()

            wordBoard.Word = "orbit";
            wordBoard.RetriesCount = 6;
        }

        private void WordBoard_TestStateChanged(object? sender, EventArgs e)
        {
        }
    }
}
