namespace Word500;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnPracticeTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("difficulty");
    }

    private async void OnLogicTestTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("game");
    }

    private async void OnDailyTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("game");
    }

    private async void OnChallengeTapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("game");
    }

    private async void OnHelpTapped(object sender, TappedEventArgs e)
    {
    }
}
