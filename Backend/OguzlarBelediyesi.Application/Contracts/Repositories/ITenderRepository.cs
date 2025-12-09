using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Application.Filters;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface ITenderRepository
{
    Task<IEnumerable<Tender>> GetAllAsync(TenderFilter? filter = null);
    Task<Tender?> GetBySlugAsync(string slug);
    Task AddAsync(Tender tender);
    Task UpdateAsync(Tender tender);
    Task DeleteAsync(Guid id);
    Task<Tender?> GetByIdAsync(Guid id);
    Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null);
}
