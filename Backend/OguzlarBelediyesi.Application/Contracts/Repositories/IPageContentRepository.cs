using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface IPageContentRepository
{
    Task<PageContent?> GetByKeyAsync(string key);
}
