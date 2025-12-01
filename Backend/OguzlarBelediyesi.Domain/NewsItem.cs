using System.Collections.Generic;

namespace OguzlarBelediyesi.Domain;

public sealed record NewsItem(
    string Image,
    string Date,
    string Title,
    string Description,
    string Slug,
    IReadOnlyList<string>? Photos = null);
