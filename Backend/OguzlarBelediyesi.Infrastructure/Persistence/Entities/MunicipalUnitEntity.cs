using System;
using OguzlarBelediyesi.Domain.Entities.Common;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Entities;

public sealed class MunicipalUnitEntity : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string Icon { get; set; } = string.Empty;
    public string StaffJson { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
}
