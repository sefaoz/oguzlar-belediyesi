using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Entities;

public sealed class MenuItemEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }

    public int Order { get; set; }

    public bool IsVisible { get; set; } = true;

    public string? Target { get; set; }
}
