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
            .OrderBy(n => n.Title)
            .ToListAsync();

        return entities.Select(Map).ToList();
    }

    public async Task<NewsItem?> GetBySlugAsync(string slug)
    {
        var entity = await _context.NewsItems
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Slug == slug);

        return entity is null ? null : Map(entity);
    }

    private static NewsItem Map(NewsEntity entity)
    {
        var photos = ParsePhotos(entity.PhotosJson);
        return new NewsItem(
            Image: entity.Image,
            Date: entity.Date,
            Title: entity.Title,
            Description: entity.Description,
            Slug: entity.Slug,
            Photos: photos.Any() ? photos : null);
    }

    private static IReadOnlyList<string> ParsePhotos(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return Array.Empty<string>();
        }

        return JsonSerializer.Deserialize<List<string>>(json, JsonOptions) ?? new List<string>();
    }
}
