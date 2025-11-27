using System;

namespace OguzlarBelediyesi.Application.Filters;

public sealed record AnnouncementFilter(
    string? SearchTerm = null,
    DateTime? From = null,
    DateTime? To = null);
