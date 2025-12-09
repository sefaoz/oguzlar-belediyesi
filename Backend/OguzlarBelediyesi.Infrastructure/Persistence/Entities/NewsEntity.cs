using System;

using System;
using OguzlarBelediyesi.Domain.Entities.Common;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Entities;

public sealed class NewsEntity : BaseEntity
{


    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Image { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string PhotosJson { get; set; } = string.Empty;

    public int ViewCount { get; set; } = 0;

    public string TagsJson { get; set; } = string.Empty;
}
