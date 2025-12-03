using System;

namespace OguzlarBelediyesi.Domain;

public sealed class Announcement
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; init; } = string.Empty;
    public string Summary { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string Date { get; init; } = string.Empty;
    public DateTime PublishedAt { get; init; } = DateTime.UtcNow;
    public string Slug { get; init; } = string.Empty;
}
