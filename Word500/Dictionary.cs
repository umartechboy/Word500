namespace Word500;

public static class Dictionary
{
    static string[]? _words;

    public static string[] GetWords()
    {
        if (_words != null)
            return _words;

        using var stream = FileSystem.OpenAppPackageFileAsync("dictionary.txt").Result;
        using var reader = new StreamReader(stream);
        var text = reader.ReadToEnd();
        _words = text.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return _words;
    }
}
