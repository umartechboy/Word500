namespace Word500
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("game", typeof(GamePage));
            Routing.RegisterRoute("difficulty", typeof(DifficultyPage));
        }
    }
}
