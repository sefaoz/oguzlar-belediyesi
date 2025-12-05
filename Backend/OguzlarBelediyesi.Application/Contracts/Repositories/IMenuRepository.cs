using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface IMenuRepository
{
    Task<IEnumerable<MenuItem>> GetAllAsync();

    Task<MenuItem?> GetByIdAsync(Guid id);

    Task AddAsync(MenuItem menuItem);

    Task UpdateAsync(MenuItem menuItem);

    Task DeleteAsync(Guid id);

    Task UpdateOrderAsync(IEnumerable<MenuItem> orderedItems);
}
