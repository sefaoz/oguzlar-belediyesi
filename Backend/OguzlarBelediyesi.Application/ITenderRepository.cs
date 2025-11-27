using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Application.Filters;

namespace OguzlarBelediyesi.Application;

public interface ITenderRepository
{
    Task<IEnumerable<Tender>> GetAllAsync(TenderFilter? filter = null);
    Task<Tender?> GetBySlugAsync(string slug);
}
