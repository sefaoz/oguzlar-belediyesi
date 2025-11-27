using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Application.Filters;

namespace OguzlarBelediyesi.Application;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetAllAsync(EventFilter? filter = null);
    Task<Event?> GetBySlugAsync(string slug);
}
