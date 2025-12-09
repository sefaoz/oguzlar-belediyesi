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

    /// <summary>
    /// Verilen metinden URL-dostu bir slug oluşturur.
    /// Türkçe karakterleri dönüştürür, özel karakterleri temizler.
    /// </summary>
    public static string GenerateSlug(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        // Küçük harfe çevir
        text = text.ToLowerInvariant();

        // Türkçe karakterleri dönüştür
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

        // Boşlukları tire ile değiştir
        text = text.Replace(' ', '-');

        // Sadece harf, rakam ve tire bırak
        text = Regex.Replace(text, @"[^a-z0-9\-]", string.Empty);

        // Ardışık tireleri tek tire yap
        text = Regex.Replace(text, @"-+", "-");

        // Baş ve sondaki tireleri kaldır
        text = text.Trim('-');

        return text;
    }

    /// <summary>
    /// Slug'a sayı ekleyerek benzersiz bir slug oluşturur.
    /// Örnek: "haber-basligi" -> "haber-basligi-2"
    /// </summary>
    public static string AppendNumber(string slug, int number)
    {
        return $"{slug}-{number}";
    }
}
