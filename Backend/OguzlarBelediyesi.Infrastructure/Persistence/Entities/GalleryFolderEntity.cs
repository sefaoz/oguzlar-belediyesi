using System;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Entities;

public sealed class GalleryFolderEntity
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string CoverImage { get; set; } = string.Empty;

    public int ImageCount { get; set; }

    public string Date { get; set; } = string.Empty;
}
