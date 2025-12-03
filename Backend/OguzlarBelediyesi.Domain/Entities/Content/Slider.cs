namespace OguzlarBelediyesi.Domain;

public sealed record Slider(
    string Id,
    string Title,
    string Description,
    string ImageUrl,
    string? Link,
    int Order,
    bool IsActive);
