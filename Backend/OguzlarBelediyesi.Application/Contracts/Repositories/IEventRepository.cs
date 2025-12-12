using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Application.Filters;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetAllAsync(EventFilter? filter = null, CancellationToken cancellationToken = default);
    Task<Event?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default);
    Task AddAsync(Event eventItem, CancellationToken cancellationToken = default);
    Task UpdateAsync(Event eventItem, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
