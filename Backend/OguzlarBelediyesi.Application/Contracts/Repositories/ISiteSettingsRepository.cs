using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain.Entities.Configuration;

namespace OguzlarBelediyesi.Application.Contracts.Repositories
{
    public interface ISiteSettingsRepository
    {
        Task<List<SiteSetting>> GetAllAsync();
        Task<List<SiteSetting>> GetByGroupAsync(string groupKey);
        Task<SiteSetting?> GetByKeyAsync(string groupKey, string key);
        Task AddAsync(SiteSetting setting);
        Task UpdateAsync(SiteSetting setting);
        Task DeleteAsync(SiteSetting setting);
    }
}
