using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface ICouncilRepository
{
    Task<IReadOnlyList<CouncilDocument>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CouncilDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(CouncilDocument document, CancellationToken cancellationToken = default);
    Task UpdateAsync(CouncilDocument document, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
