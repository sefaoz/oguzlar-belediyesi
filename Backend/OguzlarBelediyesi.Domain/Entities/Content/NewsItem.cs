using System;
using System.Collections.Generic;
using OguzlarBelediyesi.Domain.Entities.Common;

namespace OguzlarBelediyesi.Domain;

public sealed class NewsItem : BaseEntity
{
    public string Image { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public List<string>? Photos { get; set; } = new();
    public int ViewCount { get; set; }
    public List<string>? Tags { get; set; } = new();
}
