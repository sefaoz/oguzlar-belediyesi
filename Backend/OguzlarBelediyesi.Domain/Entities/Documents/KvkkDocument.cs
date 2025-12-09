using System;
using OguzlarBelediyesi.Domain.Entities.Common;

namespace OguzlarBelediyesi.Domain;

public sealed class KvkkDocument : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
}
