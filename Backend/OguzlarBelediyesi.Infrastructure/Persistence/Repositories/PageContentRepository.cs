using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Infrastructure.Persistence.Database;
using OguzlarBelediyesi.Infrastructure.Persistence.Entities;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Repositories;

public sealed class PageContentRepository : IPageContentRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly OguzlarBelediyesiDbContext _context;

    public PageContentRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<PageContent?> GetByKeyAsync(string key)
    {
        var entity = await _context.PageContents
            .AsNoTracking()
            .FirstOrDefaultAsync(pc => pc.Key == key);

        return entity is null ? null : Map(entity);
    }

    private static PageContent Map(PageContentEntity entity)
    {
        var paragraphs = ParseTextList(entity.ParagraphsJson);
        var contacts = ParseContactDetails(entity.ContactDetailsJson);
        return new PageContent(
            Title: entity.Title,
            Subtitle: entity.Subtitle,
            Paragraphs: paragraphs,
            ImageUrl: entity.ImageUrl,
            MapEmbedUrl: entity.MapEmbedUrl,
            ContactDetails: contacts.Count == 0 ? null : contacts);
    }

    private static IReadOnlyList<string> ParseTextList(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return Array.Empty<string>();
        }

        return JsonSerializer.Deserialize<List<string>>(json, JsonOptions) ?? new List<string>();
    }

    private static IReadOnlyList<ContactDetail> ParseContactDetails(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return Array.Empty<ContactDetail>();
        }

        return JsonSerializer.Deserialize<List<ContactDetail>>(json, JsonOptions) ?? new List<ContactDetail>();
    }
}
