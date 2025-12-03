using System;
using System.Collections.Generic;
using System.Linq;
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

    public async Task<IEnumerable<Announcement>> GetAllAsync(AnnouncementFilter? filter = null)
    {
        IQueryable<Announcement> query = _context.Announcements.AsNoTracking();

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var term = filter.SearchTerm.Trim().ToLowerInvariant();
                query = query.Where(a => a.Title.ToLower().Contains(term) || a.Summary.ToLower().Contains(term));
            }

            if (filter.From.HasValue)
            {
                query = query.Where(a => a.PublishedAt >= filter.From.Value);
            }

            if (filter.To.HasValue)
            {
                query = query.Where(a => a.PublishedAt <= filter.To.Value);
            }
        }

        return await query.OrderByDescending(a => a.PublishedAt).ToListAsync();
    }

    public Task<Announcement?> GetBySlugAsync(string slug)
    {
        var normalized = slug.Trim().ToLowerInvariant();
        return _context.Announcements.AsNoTracking()
            .FirstOrDefaultAsync(a => a.Slug.ToLower() == normalized);
    }
}
