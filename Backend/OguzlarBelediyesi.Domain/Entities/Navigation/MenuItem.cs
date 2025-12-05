using System;

namespace OguzlarBelediyesi.Domain;

public sealed record MenuItem(
    Guid Id,
    string Title,
    string Url,
    Guid? ParentId,
    int Order,
    bool IsVisible,
    string? Target);
