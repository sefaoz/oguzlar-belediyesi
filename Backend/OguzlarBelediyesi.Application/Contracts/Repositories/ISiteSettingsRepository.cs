using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain.Entities.Configuration;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface ISiteSettingsRepository
{
    Task<List<SiteSetting>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<SiteSetting>> GetByGroupAsync(string groupKey, CancellationToken cancellationToken = default);
    Task<SiteSetting?> GetByKeyAsync(string groupKey, string key, CancellationToken cancellationToken = default);
    Task<SiteSetting?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(SiteSetting setting, CancellationToken cancellationToken = default);
    Task UpdateAsync(SiteSetting setting, CancellationToken cancellationToken = default);
    Task DeleteAsync(SiteSetting setting, CancellationToken cancellationToken = default);
}
