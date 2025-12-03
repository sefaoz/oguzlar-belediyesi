using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Infrastructure.Persistence.Database;
using OguzlarBelediyesi.Infrastructure.Persistence.Entities;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Repositories;

public sealed class GalleryRepository : IGalleryRepository
{
    private readonly OguzlarBelediyesiDbContext _context;

    public GalleryRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GalleryFolder>> GetFoldersAsync()
    {
        var folders = await _context.GalleryFolders
            .AsNoTracking()
            .OrderBy(g => g.Title)
            .ToListAsync();

        return folders.Select(MapFolder);
    }

    public async Task<GalleryFolder?> GetFolderByIdAsync(string folderId)
    {
        var entity = await _context.GalleryFolders
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == folderId);

        return entity is null ? null : MapFolder(entity);
    }

    public async Task<GalleryFolder?> GetFolderBySlugAsync(string slug)
    {
        var entity = await _context.GalleryFolders
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Slug == slug);

        return entity is null ? null : MapFolder(entity);
    }

    public async Task<IEnumerable<GalleryImage>> GetImagesByFolderAsync(string folderId)
    {
        var images = await _context.GalleryImages
            .AsNoTracking()
            .Where(img => img.FolderId == folderId)
            .OrderBy(img => img.Id)
            .ToListAsync();

        return images.Select(MapImage);
    }

    private static GalleryFolder MapFolder(GalleryFolderEntity entity)
    {
        return new GalleryFolder(entity.Id, entity.Title, entity.Slug, entity.CoverImage, entity.ImageCount, entity.Date);
    }

    private static GalleryImage MapImage(GalleryImageEntity entity)
    {
        return new GalleryImage(entity.Id, entity.FolderId, entity.Url, entity.ThumbnailUrl, entity.Title);
    }
}
