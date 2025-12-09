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

        if (filter?.UpcomingOnly == true)
        {
             return await query.OrderBy(e => e.EventDate).ToListAsync();
        }

        return await query.OrderByDescending(e => e.CreatedDate).ToListAsync();
    }

    public Task<Event?> GetBySlugAsync(string slug)
    {
        var normalized = slug.Trim().ToLowerInvariant();
        return _context.Events.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Slug.ToLower() == normalized);
    }


    public Task<Event?> GetByIdAsync(Guid id)
    {
        return _context.Events.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null)
    {
        var query = _context.Events.AsNoTracking().Where(e => e.Slug == slug);
        if (excludeId.HasValue)
        {
            query = query.Where(e => e.Id != excludeId.Value);
        }
        return await query.AnyAsync();
    }

    public async Task AddAsync(Event eventItem)
    {
        await _context.Events.AddAsync(eventItem);
    }

    public Task UpdateAsync(Event eventItem)
    {
        _context.Events.Update(eventItem);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem != null)
        {
            _context.Events.Remove(eventItem);
        }
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
