using System;

namespace OguzlarBelediyesi.Domain;

public sealed class Event
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public string Date { get; init; } = string.Empty;
    public DateTime EventDate { get; init; } = DateTime.UtcNow;
    public string? Image { get; init; }
    public string Slug { get; init; } = string.Empty;
}
