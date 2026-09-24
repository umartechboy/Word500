namespace Word500;

public partial class DifficultyPage : ContentPage
{
    public record DifficultyLevel(
        string Name, int Letters, int Chances, string Hints,
        string Description, string Stars, Color Accent, Color BadgeBg);

    static readonly DifficultyLevel[] Levels = new[]
    {
        new DifficultyLevel(
            "Childish", 3, 8, "Unlimited",
            "Tiny words, big confidence. Perfect for warming up or playing with kids.",
            "⭐",
            Color.FromArgb("#86EFAC"), Color.FromArgb("#ECFDF5")),
        new DifficultyLevel(
            "Easy", 4, 8, "Unlimited",
            "Short words, plenty of room. A relaxed puzzle for casual play.",
            "⭐⭐",
            Color.FromArgb("#FDE047"), Color.FromArgb("#FEFCE8")),
        new DifficultyLevel(
            "Regular", 5, 8, "Unlimited",
            "The classic experience. A solid challenge for any word lover.",
            "⭐⭐⭐",
            Color.FromArgb("#512BD4"), Color.FromArgb("#EDE7FB")),
        new DifficultyLevel(
            "Hard", 5, 7, "3 max",
            "Fewer chances, same word length. Every guess counts.",
            "⭐⭐⭐⭐",
            Color.FromArgb("#FB923C"), Color.FromArgb("#FFF7ED")),
        new DifficultyLevel(
            "Legendary", 6, 8, "1 only",
            "Longer words push your vocabulary to the limit. Think carefully.",
            "⭐⭐⭐⭐⭐",
            Color.FromArgb("#F87171"), Color.FromArgb("#FEF2F2")),
    };

    DifficultyLevel _current;

    public DifficultyPage()
    {
        InitializeComponent();
        UpdateDisplay(2);
    }

    private void OnSliderChanged(object sender, ValueChangedEventArgs e)
    {
        int index = (int)Math.Round(e.NewValue);
        difficultySlider.Value = index;
        UpdateDisplay(index);
    }

    private void UpdateDisplay(int index)
    {
        _current = Levels[index];

        lblDifficultyName.Text = _current.Name;
        lblDifficultyName.TextColor = _current.Accent;
        lblStars.Text = _current.Stars;
        lblLetters.Text = $"{_current.Letters} letters";
        lblChances.Text = $"{_current.Chances} guesses";
        lblHints.Text = _current.Hints;
        lblDescription.Text = _current.Description;
        iconBadge.BackgroundColor = _current.BadgeBg;
        btnBegin.BackgroundColor = _current.Accent;
    }

    private async void OnBeginClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(
            $"game?letters={_current.Letters}&chances={_current.Chances}");
    }
}
