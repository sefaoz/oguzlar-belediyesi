using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.WebAPI.Helpers;

public static class PageContentUploadHelper
{
    private static readonly Regex ParagraphKeyRegex = new(@"^paragraphs\[(\d+)\]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex ContactDetailKeyRegex = new(@"^contactDetails\[(\d+)\]\.(label|value)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static PageContentFormData ParsePageContentForm(IFormCollection form)
    {
        var key = form["key"].FirstOrDefault() ?? string.Empty;
        var title = form["title"].FirstOrDefault() ?? string.Empty;
        var subtitle = form["subtitle"].FirstOrDefault() ?? string.Empty;
        var mapEmbedUrl = string.IsNullOrWhiteSpace(form["mapEmbedUrl"].FirstOrDefault()) ? null : form["mapEmbedUrl"].First();
        var imageUrl = string.IsNullOrWhiteSpace(form["imageUrl"].FirstOrDefault()) ? null : form["imageUrl"].First();

        var paragraphs = ParseParagraphEntries(form);
        var contactDetails = ParseContactDetails(form);

        return new PageContentFormData(key, title, subtitle, paragraphs, imageUrl, mapEmbedUrl, contactDetails);
    }

    public static string EnsureWebRootPath(IWebHostEnvironment env)
    {
        var path = env.WebRootPath;
        if (string.IsNullOrWhiteSpace(path))
        {
            path = Path.Combine(env.ContentRootPath, "wwwroot");
        }

        Directory.CreateDirectory(path);
        return path;
    }

    public static async Task<string?> SavePageContentImageAsync(IFormFile? file, string pageKey, string baseUri, string webRootPath)
    {
        if (file is null || file.Length == 0)
        {
            return null;
        }

        var sanitizedKey = SanitizePathSegment(pageKey);
        var folderPath = Path.Combine("uploads", "pages", sanitizedKey);

        var relativeUrl = await ImageHelper.SaveImageAsWebPAsync(file, folderPath, webRootPath);

        if (string.IsNullOrEmpty(relativeUrl))
        {
            return null;
        }

        var trimmedBaseUri = baseUri.TrimEnd('/');
        return $"{trimmedBaseUri}{relativeUrl}";
    }

    public static void DeleteStoredImageIfLocal(string? imageUrl, string baseUri, string webRootPath)
    {
        var relativePath = GetRelativeUploadPath(imageUrl, baseUri);
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return;
        }

        if (!relativePath.StartsWith("uploads/pages/", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var physicalPath = Path.Combine(webRootPath, relativePath.Replace('/', Path.DirectorySeparatorChar));
        if (File.Exists(physicalPath))
        {
            File.Delete(physicalPath);
        }
    }

    private static IReadOnlyList<string> ParseParagraphEntries(IFormCollection form)
    {
        var entries = new List<(int Index, string Value)>();
        foreach (var key in form.Keys)
        {
            var match = ParagraphKeyRegex.Match(key);
            if (!match.Success)
            {
                continue;
            }

            if (!int.TryParse(match.Groups[1].Value, out var index))
            {
                continue;
            }

            var paragraphValue = form[key].FirstOrDefault() ?? string.Empty;
            entries.Add((index, paragraphValue));
        }

        if (entries.Count == 0 && form.TryGetValue("paragraphs", out var fallback))
        {
            for (var i = 0; i < fallback.Count; i++)
            {
                var fallbackValue = fallback[i] ?? string.Empty;
                entries.Add((i, fallbackValue));
            }
        }

        if (entries.Count == 0)
        {
            return Array.Empty<string>();
        }

        return entries.OrderBy(entry => entry.Index).Select(entry => entry.Value).ToList();
    }

    private static IReadOnlyList<ContactDetail>? ParseContactDetails(IFormCollection form)
    {
        var contactEntries = new Dictionary<int, ContactDetailBuilder>();
        foreach (var key in form.Keys)
        {
            var match = ContactDetailKeyRegex.Match(key);
            if (!match.Success)
            {
                continue;
            }

            if (!int.TryParse(match.Groups[1].Value, out var index))
            {
                continue;
            }

            var property = match.Groups[2].Value;
            var value = form[key].FirstOrDefault();
            if (!contactEntries.TryGetValue(index, out var builder))
            {
                builder = new ContactDetailBuilder();
                contactEntries[index] = builder;
            }

            if (string.Equals(property, "label", StringComparison.OrdinalIgnoreCase))
            {
                builder.Label = value;
            }
            else
            {
                builder.Value = value;
            }
        }

        if (contactEntries.Count == 0)
        {
            return null;
        }

        var contacts = contactEntries.OrderBy(kvp => kvp.Key)
            .Select(kvp => new ContactDetail(kvp.Value.Label ?? string.Empty, kvp.Value.Value ?? string.Empty))
            .Where(contact => !string.IsNullOrWhiteSpace(contact.Label) || !string.IsNullOrWhiteSpace(contact.Value))
            .ToList();

        return contacts.Count == 0 ? null : contacts;
    }

    private static string SanitizePathSegment(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "page";
        }

        var invalidChars = Path.GetInvalidFileNameChars().Union(Path.GetInvalidPathChars()).ToArray();
        var builder = new StringBuilder();
        foreach (var ch in value.ToLowerInvariant())
        {
            if (invalidChars.Contains(ch))
            {
                continue;
            }

            if (char.IsWhiteSpace(ch))
            {
                builder.Append('-');
                continue;
            }

            builder.Append(ch);
        }

        return builder.ToString().Trim('-', '_') is { Length: > 0 } sanitized ? sanitized : "page";
    }

    private static string? GetRelativeUploadPath(string? imageUrl, string baseUri)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return null;
        }

        if (Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri))
        {
            var trimmedBaseUri = baseUri.TrimEnd('/');
            if (!imageUrl.StartsWith(trimmedBaseUri, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return uri.AbsolutePath.TrimStart('/');
        }

        if (imageUrl.StartsWith("/"))
        {
            return imageUrl.TrimStart('/');
        }

        return null;
    }

    private sealed class ContactDetailBuilder
    {
        public string? Label { get; set; }
        public string? Value { get; set; }
    }
}

public sealed record PageContentFormData(string Key, string Title, string Subtitle, IReadOnlyList<string> Paragraphs, string? ImageUrl, string? MapEmbedUrl, IReadOnlyList<ContactDetail>? ContactDetails);
