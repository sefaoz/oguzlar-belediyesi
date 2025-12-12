using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.WebAPI;

public record AnnouncementQuery(string? search, DateTime? from, DateTime? to);
public record EventQuery(string? search, bool upcomingOnly = false);
public record TenderQuery(string? search, string? status);

public record TokenResponse(string Token, string RefreshToken);
