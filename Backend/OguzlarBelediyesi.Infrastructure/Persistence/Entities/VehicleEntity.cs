namespace OguzlarBelediyesi.Infrastructure.Persistence.Entities;

public sealed class VehicleEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string Plate { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;
}
