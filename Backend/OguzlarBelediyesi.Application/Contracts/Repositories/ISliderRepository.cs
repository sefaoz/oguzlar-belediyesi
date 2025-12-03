using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface ISliderRepository
{
    Task<IEnumerable<Slider>> GetAllAsync();
    Task<Slider?> GetByIdAsync(string id);
    Task AddAsync(Slider slider);
    Task UpdateAsync(Slider slider);
    Task DeleteAsync(string id);
    Task SaveChangesAsync();
}
