using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface IMunicipalUnitRepository
{
    Task<IReadOnlyList<MunicipalUnit>> GetAllAsync();
    Task<MunicipalUnit?> GetByIdAsync(Guid id);
    Task CreateAsync(MunicipalUnit unit);
    Task UpdateAsync(MunicipalUnit unit);
    Task DeleteAsync(Guid id);
}
