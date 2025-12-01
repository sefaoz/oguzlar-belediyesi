namespace OguzlarBelediyesi.Domain;

public sealed record KvkkDocument(
    int Id,
    string Title,
    string Type,
    string FileUrl);
