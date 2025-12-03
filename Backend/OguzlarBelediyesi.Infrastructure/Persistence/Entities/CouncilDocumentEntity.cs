using System;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Entities;

public sealed class CouncilDocumentEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string? Description { get; set; }

    public string? FileUrl { get; set; }
}
