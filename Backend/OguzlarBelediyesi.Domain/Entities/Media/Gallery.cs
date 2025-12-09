using System;
using OguzlarBelediyesi.Domain.Entities.Common;

namespace OguzlarBelediyesi.Domain;

public sealed class GalleryFolder : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string CoverImage { get; set; } = string.Empty;
    public int ImageCount { get; set; }
    public string Date { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; }
}

public sealed class GalleryImage : BaseEntity
{
    public Guid FolderId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string? Title { get; set; }
}
