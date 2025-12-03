using System;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Entities;

public sealed class NewsEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Image { get; set; } = string.Empty;

    public string Date { get; set; } = string.Empty;

    public string PhotosJson { get; set; } = string.Empty;
}
