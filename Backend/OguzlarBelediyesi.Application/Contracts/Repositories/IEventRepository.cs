using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Application.Filters;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetAllAsync(EventFilter? filter = null);
    Task<Event?> GetBySlugAsync(string slug);
    Task<Event?> GetByIdAsync(Guid id);
    Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null);
    Task AddAsync(Event eventItem);
    Task UpdateAsync(Event eventItem);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}
