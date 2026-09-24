namespace Word500;

public partial class CustomGamePage : ContentPage
{
    private string _randomWord;
    private bool _isPeeking = false;

    public CustomGamePage()
    {
        InitializeComponent();
        GenerateRandomWord();
    }

    private void OnChancesChanged(object sender, ValueChangedEventArgs e)
    {
        int val = (int)Math.Round(e.NewValue);
        chancesSlider.Value = val;
        lblChancesValue.Text = $"{val} guesses";
    }

    private void OnWordLengthChanged(object sender, ValueChangedEventArgs e)
    {
        int val = (int)Math.Round(e.NewValue);
        wordLengthSlider.Value = val;
        lblWordLengthValue.Text = $"{val} letters";
        if (switchRandom.IsToggled)
            GenerateRandomWord();
    }

    private void OnRandomToggled(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            maskedWordBorder.IsVisible = true;
            entryBorder.IsVisible = false;
            wordLengthCard.IsVisible = true;
            lblWordHint.Text = "Tap to peek at the word";
            lblWordHint.IsVisible = true;
            _isPeeking = false;
            GenerateRandomWord();
        }
        else
        {
            maskedWordBorder.IsVisible = false;
            entryBorder.IsVisible = true;
            wordLengthCard.IsVisible = false;
            lblWordHint.Text = "Word length is set by what you type";
            entryWord.Text = "";
            entryWord.Focus();
        }
    }

    private void OnMaskedWordTapped(object sender, TappedEventArgs e)
    {
        if (!switchRandom.IsToggled) return;

        _isPeeking = true;
        maskedWordBorder.IsVisible = false;
        entryBorder.IsVisible = true;
        entryWord.Text = _randomWord;
        entryWord.Focus();
        lblWordHint.Text = "Edit to use a custom word, or tap away to re-mask";
    }

    private void OnWordTextChanged(object sender, TextChangedEventArgs e)
    {
        if (_isPeeking && e.NewTextValue != _randomWord)
        {
            _isPeeking = false;
            switchRandom.IsToggled = false;
            maskedWordBorder.IsVisible = false;
            entryBorder.IsVisible = true;
            wordLengthCard.IsVisible = false;
            lblWordHint.Text = "Word length is set by what you type";
        }
    }

    private void OnWordEntryUnfocused(object sender, FocusEventArgs e)
    {
        if (_isPeeking && switchRandom.IsToggled)
        {
            _isPeeking = false;
            maskedWordBorder.IsVisible = true;
            entryBorder.IsVisible = false;
            lblWordHint.Text = "Tap to peek at the word";
        }
    }

    private void GenerateRandomWord()
    {
        int length = (int)Math.Round(wordLengthSlider.Value);
        var words = Dictionary.GetWords();
        var candidates = words.Where(w => w.Length == length).ToArray();
        if (candidates.Length > 0)
            _randomWord = candidates[new Random().Next(candidates.Length)];
        else
            _randomWord = new string('?', length);

        lblMaskedWord.Text = string.Join(" ", Enumerable.Repeat("●", length));
    }

    private string GetWord()
    {
        if (switchRandom.IsToggled)
            return _randomWord;
        return entryWord.Text?.Trim().ToUpper() ?? "";
    }

    private async void OnNextClicked(object sender, EventArgs e)
    {
        string word = GetWord();
        if (string.IsNullOrEmpty(word))
        {
            await DisplayAlert("No Word", "Please enter a word or use a random one.", "OK");
            return;
        }

        int chances = (int)Math.Round(chancesSlider.Value);
        await Shell.Current.GoToAsync(
            $"game?letters={word.Length}&chances={chances}&word={word}");
    }
}
