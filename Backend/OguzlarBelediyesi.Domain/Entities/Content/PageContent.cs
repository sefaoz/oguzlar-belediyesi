using System;
using System.Collections.Generic;
using OguzlarBelediyesi.Domain.Entities.Common;

namespace OguzlarBelediyesi.Domain;

public sealed class PageContent : BaseEntity
{
    public string Key { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public List<string> Paragraphs { get; set; } = new();
    public string? ImageUrl { get; set; }
    public string? MapEmbedUrl { get; set; }
    public List<ContactDetail>? ContactDetails { get; set; }
}

public sealed record ContactDetail(string Label, string Value);
