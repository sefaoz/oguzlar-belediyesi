using System;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Entities;

public sealed class SerilogLogEntry
{
    public int Id { get; set; }

    public DateTime Timestamp { get; set; }

    public string Level { get; set; } = string.Empty;

    public string MessageTemplate { get; set; } = string.Empty;

    public string RenderedMessage { get; set; } = string.Empty;

    public string? Exception { get; set; }

    public string Properties { get; set; } = string.Empty;

    public string? Endpoint { get; set; }

    public long? DurationMs { get; set; }

    public string? Username { get; set; }
}
