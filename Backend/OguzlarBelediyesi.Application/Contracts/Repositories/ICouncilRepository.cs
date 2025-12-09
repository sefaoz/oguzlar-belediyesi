using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface ICouncilRepository
{
    Task<IReadOnlyList<CouncilDocument>> GetAllAsync();
    Task<CouncilDocument?> GetByIdAsync(Guid id);
    Task AddAsync(CouncilDocument document);
    Task UpdateAsync(CouncilDocument document);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}
