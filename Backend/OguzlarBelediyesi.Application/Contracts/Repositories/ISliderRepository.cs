using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface ISliderRepository
{
    Task<IEnumerable<Slider>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Slider?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Slider slider, CancellationToken cancellationToken = default);
    Task UpdateAsync(Slider slider, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
