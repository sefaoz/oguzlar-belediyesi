using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OguzlarBelediyesi.Application;
using OguzlarBelediyesi.Application.Filters;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Infrastructure.Database;

namespace OguzlarBelediyesi.Infrastructure.Repositories;

public sealed class EventRepository : IEventRepository
{
    private readonly OguzlarBelediyesiDbContext _context;

    public EventRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Event>> GetAllAsync(EventFilter? filter = null)
    {
        IQueryable<Event> query = _context.Events.AsNoTracking();

        if (filter?.UpcomingOnly ?? false)
        {
            var today = DateTime.UtcNow.Date;
            query = query.Where(e => e.EventDate >= today);
        }

        if (!string.IsNullOrWhiteSpace(filter?.SearchTerm))
        {
            var term = filter!.SearchTerm!.Trim().ToLowerInvariant();
            query = query.Where(e => e.Title.ToLower().Contains(term) || e.Location.ToLower().Contains(term));
        }

        return await query.OrderBy(e => e.EventDate).ToListAsync();
    }

    public Task<Event?> GetBySlugAsync(string slug)
    {
        var normalized = slug.Trim().ToLowerInvariant();
        return _context.Events.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Slug.ToLower() == normalized);
    }
}
