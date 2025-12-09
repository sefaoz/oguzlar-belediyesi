using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Infrastructure.Persistence.Database;
using OguzlarBelediyesi.Infrastructure.Persistence.Entities;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Repositories;

public sealed class NewsRepository : INewsRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly OguzlarBelediyesiDbContext _context;

    public NewsRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<NewsItem>> GetAllAsync()
    {
        var entities = await _context.NewsItems
            .AsNoTracking()
            .Where(n => !n.IsDeleted)
            .OrderByDescending(n => n.CreatedDate)
            .ToListAsync();

        return entities.Select(Map).ToList();
    }

    public async Task<NewsItem?> GetBySlugAsync(string slug)
    {
        var entity = await _context.NewsItems
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Slug == slug && !n.IsDeleted);

        return entity is null ? null : Map(entity);
    }

    public async Task<NewsItem?> GetByIdAsync(Guid id)
    {
        var entity = await _context.NewsItems
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted);

        return entity is null ? null : Map(entity);
    }

    public async Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null)
    {
        var query = _context.NewsItems.AsNoTracking().Where(n => n.Slug == slug);
        
        if (excludeId.HasValue)
        {
            query = query.Where(n => n.Id != excludeId.Value);
        }
        
        return await query.AnyAsync();
    }

    public async Task AddAsync(NewsItem newsItem)
    {
        var entity = MapToEntity(newsItem);
        await _context.NewsItems.AddAsync(entity);
    }

    public async Task UpdateAsync(NewsItem newsItem)
    {
        var entity = await _context.NewsItems.FindAsync(newsItem.Id);
        if (entity is not null)
        {
            entity.Title = newsItem.Title;
            entity.Slug = newsItem.Slug;
            entity.Description = newsItem.Description;
            entity.Image = newsItem.Image;
            entity.Date = newsItem.Date;
            entity.PhotosJson = newsItem.Photos is not null 
                ? JsonSerializer.Serialize(newsItem.Photos, JsonOptions) 
                : string.Empty;
            entity.ViewCount = newsItem.ViewCount;
            entity.TagsJson = newsItem.Tags is not null
                 ? JsonSerializer.Serialize(newsItem.Tags, JsonOptions)
                 : string.Empty;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.NewsItems.FindAsync(id);
        if (entity is not null)
        {
            entity.IsDeleted = true;
            entity.UpdateDate = DateTime.UtcNow;
        }
    }

    public async Task IncrementViewCountAsync(string slug)
    {
        var entity = await _context.NewsItems.FirstOrDefaultAsync(n => n.Slug == slug);
        if (entity is not null)
        {
            entity.ViewCount++;
        }
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    private static NewsItem Map(NewsEntity entity)
    {
        var photos = ParseStringList(entity.PhotosJson);
        var tags = ParseStringList(entity.TagsJson);
        return new NewsItem
        {
            Id = entity.Id,
            Image = entity.Image,
            Date = entity.Date,
            Title = entity.Title,
            Description = entity.Description,
            Slug = entity.Slug,
            Photos = photos.Any() ? photos.ToList() : null,
            ViewCount = entity.ViewCount,
            Tags = tags.Any() ? tags.ToList() : null
        };
    }

    private static NewsEntity MapToEntity(NewsItem newsItem)
    {
        return new NewsEntity
        {
            Id = newsItem.Id,
            Title = newsItem.Title,
            Slug = newsItem.Slug,
            Description = newsItem.Description,
            Image = newsItem.Image,
            Date = newsItem.Date,
            PhotosJson = newsItem.Photos is not null 
                ? JsonSerializer.Serialize(newsItem.Photos, JsonOptions) 
                : string.Empty,
            ViewCount = newsItem.ViewCount,
            TagsJson = newsItem.Tags is not null
                 ? JsonSerializer.Serialize(newsItem.Tags, JsonOptions)
                 : string.Empty
        };
    }

    private static IReadOnlyList<string> ParseStringList(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return Array.Empty<string>();
        }

        return JsonSerializer.Deserialize<List<string>>(json, JsonOptions) ?? new List<string>();
    }
}
