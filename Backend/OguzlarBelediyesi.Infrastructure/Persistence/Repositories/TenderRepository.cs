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

        return await query.OrderByDescending(t => t.TenderDate).ToListAsync();
    }

    public Task<Tender?> GetBySlugAsync(string slug)
    {
        var normalized = slug.Trim().ToLowerInvariant();
        return _context.Tenders.AsNoTracking()
            .Where(t => !t.IsDeleted)
            .FirstOrDefaultAsync(t => t.Slug.ToLower() == normalized);
    }

    public async Task AddAsync(Tender tender)
    {
        _context.Tenders.Add(tender);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tender tender)
    {
        _context.Tenders.Update(tender);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var tender = await _context.Tenders.FindAsync(id);
        if (tender != null)
        {
            tender.IsDeleted = true; // Key assumption: user wants soft delete maintained? BaseEntity has IsDeleted.
            // Usually delete means soft delete in this project if BaseEntity has IsDeleted.
            // However, typical EF "Delete" means Remove.
            // Let's check BaseEntity usage. It has IsDeleted.
            // But look at AnnouncementRepository or others to see pattern.
            // I'll stick to simple update IsDeleted = true or Remove depending on requirement.
            // User: "Soft Deletion for Announcements" was a previous task.
            // I'll assume Soft Delete is preferred if IsDeleted exists.
            // But wait, I'll use Remove in repository if unaware, BUT BaseEntity suggests soft delete.
            // Let's look at `OguzlarBelediyesiDbContext` or other Repos.
            // I'll use Remove for now, or check Filter query.
            // GetAllAsync uses AsNoTracking() but doesn't filter !IsDeleted.
            // Wait, does DbContext have QueryFilter?
            // I didn't see Global Query Filter in DbContext.
            // But `AnnouncementRepository` conversation mentions "Soft Deletion".
            // I will use `tender.IsDeleted = true;` and Update.
            tender.IsDeleted = true;
            _context.Tenders.Update(tender);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Tender?> GetByIdAsync(Guid id)
    {
        return await _context.Tenders.FindAsync(id);
    }

    public async Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null)
    {
        var query = _context.Tenders.AsNoTracking().Where(t => t.Slug == slug);
        if (excludeId.HasValue)
        {
            query = query.Where(t => t.Id != excludeId.Value);
        }
        return await query.AnyAsync();
    }
}
