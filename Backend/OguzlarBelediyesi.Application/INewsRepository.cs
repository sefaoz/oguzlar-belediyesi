using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application;

public interface INewsRepository
{
    Task<IEnumerable<NewsItem>> GetAllAsync();
    Task<NewsItem?> GetBySlugAsync(string slug);
}
