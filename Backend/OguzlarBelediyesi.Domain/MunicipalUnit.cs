using System.Collections.Generic;

namespace OguzlarBelediyesi.Domain;

public sealed record UnitStaff(
    string Name,
    string Title,
    string? ImageUrl = null);

public sealed record MunicipalUnit(
    string Id,
    string Title,
    string? Content = null,
    string Icon = "",
    IReadOnlyList<UnitStaff>? Staff = null);
