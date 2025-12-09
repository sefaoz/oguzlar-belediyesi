using Microsoft.EntityFrameworkCore;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain.Entities.Configuration;
using OguzlarBelediyesi.Infrastructure.Persistence.Database;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Repositories
{
    public class SiteSettingsRepository : ISiteSettingsRepository
    {
        private readonly OguzlarBelediyesiDbContext _context;

        public SiteSettingsRepository(OguzlarBelediyesiDbContext context)
        {
            _context = context;
        }

        public async Task<List<SiteSetting>> GetAllAsync()
        {
            return await _context.SiteSettings.AsNoTracking().ToListAsync();
        }

        public async Task<List<SiteSetting>> GetByGroupAsync(string groupKey)
        {
            return await _context.SiteSettings
                .Where(s => s.GroupKey == groupKey)
                .OrderBy(s => s.Order)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SiteSetting?> GetByKeyAsync(string groupKey, string key)
        {
            return await _context.SiteSettings
                .FirstOrDefaultAsync(s => s.GroupKey == groupKey && s.Key == key);
        }

        public async Task AddAsync(SiteSetting setting)
        {
            await _context.SiteSettings.AddAsync(setting);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SiteSetting setting)
        {
            _context.SiteSettings.Update(setting);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(SiteSetting setting)
        {
            _context.SiteSettings.Remove(setting);
            await _context.SaveChangesAsync();
        }
    }
}
