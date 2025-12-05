using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface IPageContentRepository
{
    Task<IEnumerable<PageContent>> GetAllAsync();
    Task<PageContent?> GetByKeyAsync(string key);
    Task<PageContent?> GetByIdAsync(Guid id);
    Task AddAsync(PageContent pageContent);
    Task UpdateAsync(PageContent pageContent);
    Task DeleteAsync(Guid id);
}
