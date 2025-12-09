using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Application.Filters;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface IAnnouncementRepository
{
    Task<IEnumerable<Announcement>> GetAllAsync(AnnouncementFilter? filter = null);
    Task<Announcement?> GetBySlugAsync(string slug);
    Task<Announcement?> GetByIdAsync(Guid id);
    Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null);
    Task AddAsync(Announcement announcement);
    Task UpdateAsync(Announcement announcement);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}
