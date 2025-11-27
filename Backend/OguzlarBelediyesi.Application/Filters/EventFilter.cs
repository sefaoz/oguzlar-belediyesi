namespace OguzlarBelediyesi.Application.Filters;

public sealed record EventFilter(
    string? SearchTerm = null,
    bool UpcomingOnly = false);
