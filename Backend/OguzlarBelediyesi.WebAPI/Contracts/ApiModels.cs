using System;
using System.Collections.Generic;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.WebAPI;

public record AnnouncementQuery(string? search, DateTime? from, DateTime? to);
public record EventQuery(string? search, bool upcomingOnly = false);
public record TenderQuery(string? search, string? status);
public record LoginRequest(string Username, string Password);
public record SliderRequest(string Title, string Description, string ImageUrl, string? Link, int Order, bool IsActive);
public record PageContentRequest(string Key, string Title, string Subtitle, IReadOnlyList<string> Paragraphs, string? ImageUrl, string? MapEmbedUrl, IReadOnlyList<ContactDetail>? ContactDetails);
public record MenuRequest(string Title, string Url, Guid? ParentId, int Order, bool IsVisible, string? Target);
public record MenuOrderRequest(Guid Id, int Order, Guid? ParentId);
