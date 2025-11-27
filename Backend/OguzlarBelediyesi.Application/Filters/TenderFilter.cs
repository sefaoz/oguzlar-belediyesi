namespace OguzlarBelediyesi.Application.Filters;

public sealed record TenderFilter(
    string? SearchTerm = null,
    string? Status = null);
