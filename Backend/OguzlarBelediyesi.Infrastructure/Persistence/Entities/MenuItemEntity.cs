using System;
using OguzlarBelediyesi.Domain.Entities.Common;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Entities;

public sealed class MenuItemEntity : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
    public int Order { get; set; }
    public bool IsVisible { get; set; } = true;
    public string? Target { get; set; }
}
