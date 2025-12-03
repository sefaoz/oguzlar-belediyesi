namespace OguzlarBelediyesi.Infrastructure.Persistence.Entities;

public sealed class GalleryImageEntity
{
    public string Id { get; set; } = string.Empty;

    public string FolderId { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public string ThumbnailUrl { get; set; } = string.Empty;

    public string? Title { get; set; }
}
