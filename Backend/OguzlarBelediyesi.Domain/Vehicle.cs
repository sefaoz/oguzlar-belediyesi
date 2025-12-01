namespace OguzlarBelediyesi.Domain;

public sealed record Vehicle(
    int Id,
    string Name,
    string Type,
    string Plate,
    string Description,
    string ImageUrl);
