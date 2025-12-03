using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface INewsRepository
{
    Task<IEnumerable<NewsItem>> GetAllAsync();
    Task<NewsItem?> GetBySlugAsync(string slug);
}
