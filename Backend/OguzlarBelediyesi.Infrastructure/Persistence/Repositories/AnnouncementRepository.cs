using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Application.Filters;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Infrastructure.Persistence.Database;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Repositories;

public sealed class AnnouncementRepository : IAnnouncementRepository
{
    private readonly OguzlarBelediyesiDbContext _context;

    public AnnouncementRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Announcement>> GetAllAsync(AnnouncementFilter? filter = null, CancellationToken cancellationToken = default)
    {
        IQueryable<Announcement> query = _context.Announcements.AsNoTracking().Where(a => !a.IsDeleted);

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var term = filter.SearchTerm.Trim().ToLowerInvariant();
                query = query.Where(a => a.Title.ToLower().Contains(term) || a.Summary.ToLower().Contains(term));
            }

            if (filter.From.HasValue)
            {
                query = query.Where(a => a.Date >= filter.From.Value);
            }

            if (filter.To.HasValue)
            {
                query = query.Where(a => a.Date <= filter.To.Value);
            }
        }

        return await query.OrderByDescending(a => a.CreatedDate).ToListAsync(cancellationToken);
    }

    public async Task<Announcement?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var normalized = slug.Trim().ToLowerInvariant();
        return await _context.Announcements.AsNoTracking()
            .FirstOrDefaultAsync(a => a.Slug.ToLower() == normalized && !a.IsDeleted, cancellationToken);
    }

    public async Task<Announcement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Announcements.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);
    }

    public async Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Announcements.AsNoTracking().Where(a => a.Slug == slug && !a.IsDeleted);
        if (excludeId.HasValue)
        {
            query = query.Where(a => a.Id != excludeId.Value);
        }
        return await query.AnyAsync(cancellationToken);
    }

    public async Task AddAsync(Announcement announcement, CancellationToken cancellationToken = default)
    {
        await _context.Announcements.AddAsync(announcement, cancellationToken);
    }

    public Task UpdateAsync(Announcement announcement, CancellationToken cancellationToken = default)
    {
        _context.Announcements.Update(announcement);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Announcements.FindAsync(new object[] { id }, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.UpdateDate = DateTime.UtcNow;
        }
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
