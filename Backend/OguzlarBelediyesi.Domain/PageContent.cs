namespace OguzlarBelediyesi.Domain;

public sealed record PageContent(
    string Title,
    string Subtitle,
    IReadOnlyList<string> Paragraphs,
    string? ImageUrl = null,
    string? MapEmbedUrl = null,
    IReadOnlyList<ContactDetail>? ContactDetails = null);

public sealed record ContactDetail(string Label, string Value);
