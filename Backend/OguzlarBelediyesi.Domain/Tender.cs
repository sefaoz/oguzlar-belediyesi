using System;

namespace OguzlarBelediyesi.Domain;

public sealed class Tender
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Date { get; init; } = string.Empty;
    public DateTime PublishedAt { get; init; } = DateTime.UtcNow;
    public string RegistrationNumber { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public decimal? EstimatedValue { get; init; }
    public string Slug { get; init; } = string.Empty;
}
