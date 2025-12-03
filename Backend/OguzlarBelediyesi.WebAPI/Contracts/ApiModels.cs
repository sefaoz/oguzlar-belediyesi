using System;

namespace OguzlarBelediyesi.WebAPI;

public record AnnouncementQuery(string? search, DateTime? from, DateTime? to);
public record EventQuery(string? search, bool upcomingOnly = false);
public record TenderQuery(string? search, string? status);
public record LoginRequest(string Username, string Password);
public record SliderRequest(string Title, string Description, string ImageUrl, string? Link, int Order, bool IsActive);
