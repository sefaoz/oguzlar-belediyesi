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

public sealed class EventRepository : IEventRepository
{
    private readonly OguzlarBelediyesiDbContext _context;

    public EventRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Event>> GetAllAsync(EventFilter? filter = null, CancellationToken cancellationToken = default)
    {
        IQueryable<Event> query = _context.Events.AsNoTracking().Where(e => !e.IsDeleted);

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
             return await query.OrderBy(e => e.EventDate).ToListAsync(cancellationToken);
        }

        return await query.OrderByDescending(e => e.CreatedDate).ToListAsync(cancellationToken);
    }

    public async Task<Event?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var normalized = slug.Trim().ToLowerInvariant();
        return await _context.Events.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Slug.ToLower() == normalized && !e.IsDeleted, cancellationToken);
    }


    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Events.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted, cancellationToken);
    }

    public async Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Events.AsNoTracking().Where(e => e.Slug == slug && !e.IsDeleted);
        if (excludeId.HasValue)
        {
            query = query.Where(e => e.Id != excludeId.Value);
        }
        return await query.AnyAsync(cancellationToken);
    }

    public async Task AddAsync(Event eventItem, CancellationToken cancellationToken = default)
    {
        await _context.Events.AddAsync(eventItem, cancellationToken);
    }

    public Task UpdateAsync(Event eventItem, CancellationToken cancellationToken = default)
    {
        _context.Events.Update(eventItem);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var eventItem = await _context.Events.FindAsync(new object[] { id }, cancellationToken);
        if (eventItem != null)
        {
            eventItem.IsDeleted = true;
            eventItem.UpdateDate = DateTime.UtcNow;
        }
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
