using System;

namespace OguzlarBelediyesi.Domain;

public sealed record User
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Username { get; init; } = string.Empty;
    public string PasswordHash { get; init; } = string.Empty;
    public string Role { get; init; } = "Admin";
}
