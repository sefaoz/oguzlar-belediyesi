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

public sealed class TenderRepository : ITenderRepository
{
    private readonly OguzlarBelediyesiDbContext _context;

    public TenderRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tender>> GetAllAsync(TenderFilter? filter = null, CancellationToken cancellationToken = default)
    {
        IQueryable<Tender> query = _context.Tenders.AsNoTracking().Where(t => !t.IsDeleted);

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

        return await query.OrderByDescending(t => t.TenderDate).ToListAsync(cancellationToken);
    }

    public async Task<Tender?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var normalized = slug.Trim().ToLowerInvariant();
        return await _context.Tenders.AsNoTracking()
            .Where(t => !t.IsDeleted)
            .FirstOrDefaultAsync(t => t.Slug.ToLower() == normalized, cancellationToken);
    }

    public async Task AddAsync(Tender tender, CancellationToken cancellationToken = default)
    {
        _context.Tenders.Add(tender);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Tender tender, CancellationToken cancellationToken = default)
    {
        _context.Tenders.Update(tender);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tender = await _context.Tenders.FindAsync(new object[] { id }, cancellationToken);
        if (tender != null)
        {
            tender.IsDeleted = true;
            _context.Tenders.Update(tender);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<Tender?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tenders.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Tenders.AsNoTracking().Where(t => t.Slug == slug);
        if (excludeId.HasValue)
        {
            query = query.Where(t => t.Id != excludeId.Value);
        }
        return await query.AnyAsync(cancellationToken);
    }
}
