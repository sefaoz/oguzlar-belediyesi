using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application;

public interface IPageContentRepository
{
    Task<PageContent?> GetByKeyAsync(string key);
}
