namespace OguzlarBelediyesi.Domain;

public sealed record CouncilDocument(
    int Id,
    string Title,
    string Type,
    DateTime Date,
    string? Description = null,
    string? FileUrl = null);
