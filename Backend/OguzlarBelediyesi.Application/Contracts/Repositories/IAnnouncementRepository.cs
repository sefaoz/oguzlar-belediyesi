using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Application.Filters;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface IAnnouncementRepository
{
    Task<IEnumerable<Announcement>> GetAllAsync(AnnouncementFilter? filter = null);
    Task<Announcement?> GetBySlugAsync(string slug);
}
