using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.WebAPI;

public record AnnouncementQuery(string? search, DateTime? from, DateTime? to);
public record EventQuery(string? search, bool upcomingOnly = false);
public record TenderQuery(string? search, string? status);
public record LoginRequest(string Username, string Password);
public record TokenResponse(string Token, string RefreshToken);
public record RefreshTokenRequest(string Token, string RefreshToken);
public record SliderRequest(string? Title, string? Description, string? ImageUrl, string? Link, int Order, bool IsActive);
public record PageContentRequest(string Key, string Title, string Subtitle, IReadOnlyList<string> Paragraphs, string? ImageUrl, string? MapEmbedUrl, IReadOnlyList<ContactDetail>? ContactDetails);
public record MenuRequest(string Title, string Url, Guid? ParentId, int Order, bool IsVisible, string? Target);
public record MenuOrderRequest(Guid Id, int Order, Guid? ParentId);
public record NewsRequest(string Title, string Description, DateTime Date, string? Image, IReadOnlyList<string>? Tags, IReadOnlyList<string>? Photos);
public record AnnouncementRequest(string Title, string Summary, string Content, string Category, DateTime Date);
public record EventRequest(string Title, string Description, string Location, DateTime EventDate, string EventTime, string? Image);
public record TenderRequest(string? Title, string? Description, string? RegistrationNumber, string? Status, DateTime? TenderDate, string? DocumentsJson);
public record VehicleRequest(string Name, string Type, string Plate, string Description, string? ImageUrl);
public record GalleryFolderRequest(string Title, string Date, bool IsFeatured, bool IsActive);
public record GalleryImageRequest(Guid FolderId, string? Title);
public record UserRequest(string Username, string? Password, string Role);
public record SiteSettingRequest(string Key, string Value, string GroupKey, string? Description, int Order);

public class GalleryFolderFormRequest
{
    public string Title { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; }
    public IFormFile? CoverImage { get; set; }
}
