using System.Text;
using System.Text.RegularExpressions;

namespace OguzlarBelediyesi.WebAPI.Helpers;

public static class SlugHelper
{
    private static readonly Dictionary<char, string> TurkishCharMap = new()
    {
        { 'ç', "c" }, { 'Ç', "c" },
        { 'ğ', "g" }, { 'Ğ', "g" },
        { 'ı', "i" }, { 'İ', "i" },
        { 'ö', "o" }, { 'Ö', "o" },
        { 'ş', "s" }, { 'Ş', "s" },
        { 'ü', "u" }, { 'Ü', "u" }
    };

    public static string GenerateSlug(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        text = text.ToLowerInvariant();

        var sb = new StringBuilder();
        foreach (var c in text)
        {
            if (TurkishCharMap.TryGetValue(c, out var replacement))
            {
                sb.Append(replacement);
            }
            else
            {
                sb.Append(c);
            }
        }
        text = sb.ToString();

        text = text.Replace(' ', '-');

        text = Regex.Replace(text, @"[^a-z0-9\-]", string.Empty);

        text = Regex.Replace(text, @"-+", "-");

        text = text.Trim('-');

        return text;
    }

    public static string AppendNumber(string slug, int number)
    {
        return $"{slug}-{number}";
    }
}
