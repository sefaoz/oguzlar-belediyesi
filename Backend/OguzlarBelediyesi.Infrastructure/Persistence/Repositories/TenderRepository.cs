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

public sealed class TenderRepository : ITenderRepository
{
    private readonly OguzlarBelediyesiDbContext _context;

    public TenderRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tender>> GetAllAsync(TenderFilter? filter = null)
    {
        IQueryable<Tender> query = _context.Tenders.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter?.Status))
        {
            var status = filter!.Status!.Trim().ToLowerInvariant();
            query = query.Where(t => t.Status.ToLower() == status);
        }

        if (!string.IsNullOrWhiteSpace(filter?.SearchTerm))
        {
            var term = filter!.SearchTerm!.Trim().ToLowerInvariant();
            query = query.Where(t => t.Title.ToLower().Contains(term) || t.Description.ToLower().Contains(term));
        }

        return await query.OrderByDescending(t => t.PublishedAt).ToListAsync();
    }

    public Task<Tender?> GetBySlugAsync(string slug)
    {
        var normalized = slug.Trim().ToLowerInvariant();
        return _context.Tenders.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Slug.ToLower() == normalized);
    }
}
