using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain.Entities.Configuration;
using OguzlarBelediyesi.Infrastructure.Persistence.Database;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Repositories;

public class SiteSettingsRepository : ISiteSettingsRepository
{
    private readonly OguzlarBelediyesiDbContext _context;

    public SiteSettingsRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<List<SiteSetting>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SiteSettings.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<List<SiteSetting>> GetByGroupAsync(string groupKey, CancellationToken cancellationToken = default)
    {
        return await _context.SiteSettings
            .Where(s => s.GroupKey == groupKey)
            .OrderBy(s => s.Order)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<SiteSetting?> GetByKeyAsync(string groupKey, string key, CancellationToken cancellationToken = default)
    {
        return await _context.SiteSettings
            .FirstOrDefaultAsync(s => s.GroupKey == groupKey && s.Key == key, cancellationToken);
    }

    public async Task<SiteSetting?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SiteSettings.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task AddAsync(SiteSetting setting, CancellationToken cancellationToken = default)
    {
        await _context.SiteSettings.AddAsync(setting, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(SiteSetting setting, CancellationToken cancellationToken = default)
    {
        _context.SiteSettings.Update(setting);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(SiteSetting setting, CancellationToken cancellationToken = default)
    {
        _context.SiteSettings.Remove(setting);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
