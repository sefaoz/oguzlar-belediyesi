using System;
using System.Linq;
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

    public async Task<IEnumerable<PageContent>> GetAllAsync()
    {
        var entities = await _context.PageContents
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(Map);
    }

    public async Task<PageContent?> GetByKeyAsync(string key)
    {
        var entity = await _context.PageContents
            .AsNoTracking()
            .FirstOrDefaultAsync(pc => pc.Key == key);

        return entity is null ? null : Map(entity);
    }

    public async Task<PageContent?> GetByIdAsync(Guid id)
    {
        var entity = await _context.PageContents
            .AsNoTracking()
            .FirstOrDefaultAsync(pc => pc.Id == id);

        return entity is null ? null : Map(entity);
    }

    public async Task AddAsync(PageContent pageContent)
    {
        var entity = new PageContentEntity
        {
            Id = pageContent.Id,
            Key = pageContent.Key,
            Title = pageContent.Title,
            Subtitle = pageContent.Subtitle,
            ParagraphsJson = JsonSerializer.Serialize(pageContent.Paragraphs, JsonOptions),
            ImageUrl = pageContent.ImageUrl,
            MapEmbedUrl = pageContent.MapEmbedUrl,
            ContactDetailsJson = pageContent.ContactDetails is null ? null : JsonSerializer.Serialize(pageContent.ContactDetails, JsonOptions)
        };

        await _context.PageContents.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PageContent pageContent)
    {
        var entity = await _context.PageContents.FirstOrDefaultAsync(pc => pc.Id == pageContent.Id);
        if (entity is null)
        {
            return;
        }

        entity.Key = pageContent.Key;
        entity.Title = pageContent.Title;
        entity.Subtitle = pageContent.Subtitle;
        entity.ParagraphsJson = JsonSerializer.Serialize(pageContent.Paragraphs, JsonOptions);
        entity.ImageUrl = pageContent.ImageUrl;
        entity.MapEmbedUrl = pageContent.MapEmbedUrl;
        entity.ContactDetailsJson = pageContent.ContactDetails is null ? null : JsonSerializer.Serialize(pageContent.ContactDetails, JsonOptions);

        _context.PageContents.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.PageContents.FirstOrDefaultAsync(pc => pc.Id == id);
        if (entity is not null)
        {
            _context.PageContents.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    private static PageContent Map(PageContentEntity entity)
    {
        var paragraphs = ParseTextList(entity.ParagraphsJson);
        var contacts = ParseContactDetails(entity.ContactDetailsJson);
        return new PageContent
        {
            Id = entity.Id,
            Key = entity.Key,
            Title = entity.Title,
            Subtitle = entity.Subtitle,
            Paragraphs = paragraphs.ToList(),
            ImageUrl = entity.ImageUrl,
            MapEmbedUrl = entity.MapEmbedUrl,
            ContactDetails = contacts.Count == 0 ? null : contacts.ToList()
        };
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
