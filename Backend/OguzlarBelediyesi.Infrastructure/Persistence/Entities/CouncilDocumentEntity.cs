using System;
using OguzlarBelediyesi.Domain.Entities.Common;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Entities;

public sealed class CouncilDocumentEntity : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public string? FileUrl { get; set; }
}
