using System;
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

    public async Task<GalleryFolder?> GetFolderByIdAsync(Guid folderId)
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

    public async Task<IEnumerable<GalleryImage>> GetImagesByFolderAsync(Guid folderId)
    {
        var images = await _context.GalleryImages
            .AsNoTracking()
            .Where(img => img.FolderId == folderId)
            .OrderBy(img => img.Id)
            .ToListAsync();

        return images.Select(MapImage);
    }



    public async Task DeleteFolderAsync(Guid folderId)
    {
        var entity = await _context.GalleryFolders.FindAsync(folderId);
        if (entity != null)
        {
            _context.GalleryFolders.Remove(entity);
            // Optionally delete images? Usually cascading delete handles this in database or we do it manually.
            // Assuming EF cascade or manual cleanup. Let's just remove folder for now.
             var images = await _context.GalleryImages.Where(i => i.FolderId == folderId).ToListAsync();
             _context.GalleryImages.RemoveRange(images);

            await _context.SaveChangesAsync();
        }
    }

    public async Task AddImageAsync(GalleryImage image)
    {
        var entity = new GalleryImageEntity
        {
            Id = image.Id,
            FolderId = image.FolderId,
            Url = image.Url,
            ThumbnailUrl = image.ThumbnailUrl,
            Title = image.Title
        };

        _context.GalleryImages.Add(entity);
        
        // Update folder image count
        var folder = await _context.GalleryFolders.FindAsync(image.FolderId);
        if (folder != null)
        {
            folder.ImageCount++;
            if (string.IsNullOrEmpty(folder.CoverImage))
            {
                folder.CoverImage = image.Url;
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteImageAsync(Guid imageId)
    {
        var entity = await _context.GalleryImages.FindAsync(imageId);
        if (entity != null)
        {
            _context.GalleryImages.Remove(entity);
            
             // Update folder image count
            var folder = await _context.GalleryFolders.FindAsync(entity.FolderId);
            if (folder != null)
            {
                folder.ImageCount = folder.ImageCount > 0 ? folder.ImageCount - 1 : 0;
            }

            await _context.SaveChangesAsync();
        }
    }

    public async Task CreateFolderAsync(GalleryFolder folder)
    {
        var entity = new GalleryFolderEntity
        {
            Id = folder.Id,
            Title = folder.Title,
            Slug = folder.Slug,
            CoverImage = folder.CoverImage,
            ImageCount = folder.ImageCount,
            Date = folder.Date,
            IsFeatured = folder.IsFeatured,
            IsActive = folder.IsActive
        };

        _context.GalleryFolders.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFolderAsync(GalleryFolder folder)
    {
        var entity = await _context.GalleryFolders.FirstOrDefaultAsync(f => f.Id == folder.Id);
        if (entity == null) return;

        entity.Title = folder.Title;
        entity.Slug = folder.Slug;
        entity.CoverImage = folder.CoverImage;
        entity.Date = folder.Date;
        entity.IsFeatured = folder.IsFeatured;
        entity.IsActive = folder.IsActive;
        
        _context.GalleryFolders.Update(entity);
        await _context.SaveChangesAsync();
    }

    private static GalleryFolder MapFolder(GalleryFolderEntity entity)
    {
        return new GalleryFolder
        {
            Id = entity.Id,
            Title = entity.Title,
            Slug = entity.Slug,
            CoverImage = entity.CoverImage,
            ImageCount = entity.ImageCount,
            Date = entity.Date,
            IsFeatured = entity.IsFeatured,
            IsActive = entity.IsActive
        };
    }

    private static GalleryImage MapImage(GalleryImageEntity entity)
    {
        return new GalleryImage
        {
            Id = entity.Id,
            FolderId = entity.FolderId,
            Url = entity.Url,
            ThumbnailUrl = entity.ThumbnailUrl,
            Title = entity.Title
        };
    }
}
