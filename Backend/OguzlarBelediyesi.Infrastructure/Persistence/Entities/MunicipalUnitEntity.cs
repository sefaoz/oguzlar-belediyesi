namespace OguzlarBelediyesi.Infrastructure.Persistence.Entities;

public sealed class MunicipalUnitEntity
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string? Content { get; set; }

    public string Icon { get; set; } = string.Empty;

    public string StaffJson { get; set; } = string.Empty;
}
