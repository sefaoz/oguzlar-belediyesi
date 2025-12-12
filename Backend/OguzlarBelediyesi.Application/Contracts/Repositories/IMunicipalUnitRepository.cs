using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface IMunicipalUnitRepository
{
    Task<IReadOnlyList<MunicipalUnit>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<MunicipalUnit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task CreateAsync(MunicipalUnit unit, CancellationToken cancellationToken = default);
    Task UpdateAsync(MunicipalUnit unit, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
