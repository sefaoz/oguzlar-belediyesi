using System;
using OguzlarBelediyesi.Domain.Entities.Common;

namespace OguzlarBelediyesi.Domain;

public sealed class Event : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public DateTime EventDate { get; set; } = DateTime.UtcNow;
    public string? Image { get; set; }
    public string Slug { get; set; } = string.Empty;
}
