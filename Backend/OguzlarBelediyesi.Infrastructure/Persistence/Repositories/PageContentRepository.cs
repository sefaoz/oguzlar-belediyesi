using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
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

    public async Task<IEnumerable<PageContent>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.PageContents
            .AsNoTracking()
            .Where(pc => !pc.IsDeleted)
            .ToListAsync(cancellationToken);

        return entities.Select(Map);
    }

    public async Task<PageContent?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        var entity = await _context.PageContents
            .AsNoTracking()
            .FirstOrDefaultAsync(pc => pc.Key == key && !pc.IsDeleted, cancellationToken);

        return entity is null ? null : Map(entity);
    }

    public async Task<PageContent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.PageContents
            .AsNoTracking()
            .FirstOrDefaultAsync(pc => pc.Id == id && !pc.IsDeleted, cancellationToken);

        return entity is null ? null : Map(entity);
    }

    public async Task AddAsync(PageContent pageContent, CancellationToken cancellationToken = default)
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

        await _context.PageContents.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(PageContent pageContent, CancellationToken cancellationToken = default)
    {
        var entity = await _context.PageContents.FirstOrDefaultAsync(pc => pc.Id == pageContent.Id, cancellationToken);
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
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.PageContents.FirstOrDefaultAsync(pc => pc.Id == id, cancellationToken);
        if (entity is not null)
        {
            entity.IsDeleted = true;
            entity.UpdateDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
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
