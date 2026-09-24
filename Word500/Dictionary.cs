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
        var text = reader.ReadToEnd().ToUpper();
        _words = text.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return _words;
    }

    internal static string Squible(string toScramble, string toAvoid, bool hard)
    {
        if (hard) 
            return StrictSquible(toScramble, toAvoid);
        var toReturn = toScramble;
        List<string> potentials = new();
        foreach (var word in GetWords())
        {
            if (word == toAvoid)
                continue;
            if (word.Length < toScramble.Length)
                continue;
            if (word.Length != toAvoid.Length)
                continue;
            // length is same
            int matched = 0;
            foreach (var chr in toScramble)
                if (!word.Contains(chr))
                    break;
                else
                    matched++;
            if (matched == toScramble.Length)
                potentials.Add(word);
        }
        if (potentials.Count > 0)
            return potentials[new Random().Next(0, potentials.Count)];
        return toScramble;
    }
    internal static string StrictSquible(string toScramble, string toAvoid)
    {
        var toReturn = toScramble;
        List<string> potentials = new();
        foreach (var word in GetWords())
        {
            if (word == toAvoid)
                continue;
            if (word.Length < toScramble.Length)
                continue;
            if (word.Length != toAvoid.Length)
                continue;
            // length is same
            int matched = 0;
            for (int i = 0; i < toScramble.Length; i++)
            {
                var chr = toScramble[i];
                if (word[i] != chr && chr != '_')
                    break;
                else
                    matched++;
            }
            if (matched == toScramble.Length)
                potentials.Add(word);
        }
        if (potentials.Count > 0)
            return potentials[new Random().Next(0, potentials.Count)];
        return toScramble;
    }
}
