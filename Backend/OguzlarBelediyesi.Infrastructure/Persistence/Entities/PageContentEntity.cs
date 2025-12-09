using System;

using OguzlarBelediyesi.Domain.Entities.Common;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Entities;

public sealed class PageContentEntity : BaseEntity
{


    public string Key { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Subtitle { get; set; } = string.Empty;

    public string ParagraphsJson { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    public string? MapEmbedUrl { get; set; }

    public string? ContactDetailsJson { get; set; }
}
