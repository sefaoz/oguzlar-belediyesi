using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface IMenuRepository
{
    Task<IEnumerable<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<MenuItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(MenuItem menuItem, CancellationToken cancellationToken = default);
    Task UpdateAsync(MenuItem menuItem, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateOrderAsync(IEnumerable<MenuItem> orderedItems, CancellationToken cancellationToken = default);
}
