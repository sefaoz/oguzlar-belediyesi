using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Application.Filters;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface ITenderRepository
{
    Task<IEnumerable<Tender>> GetAllAsync(TenderFilter? filter = null, CancellationToken cancellationToken = default);
    Task<Tender?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task AddAsync(Tender tender, CancellationToken cancellationToken = default);
    Task UpdateAsync(Tender tender, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Tender?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default);
}
