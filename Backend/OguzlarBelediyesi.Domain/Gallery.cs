namespace OguzlarBelediyesi.Domain;

public sealed record GalleryFolder(
    string Id,
    string Title,
    string Slug,
    string CoverImage,
    int ImageCount,
    string Date);

public sealed record GalleryImage(
    string Id,
    string FolderId,
    string Url,
    string ThumbnailUrl,
    string? Title = null);
