using System;
using System.Collections.Generic;
using OguzlarBelediyesi.Domain.Entities.Common;

namespace OguzlarBelediyesi.Domain;

public sealed record UnitStaff(
    string Name,
    string Title,
    string? ImageUrl = null);

public sealed class MunicipalUnit : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string Icon { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public List<UnitStaff>? Staff { get; set; }
}
