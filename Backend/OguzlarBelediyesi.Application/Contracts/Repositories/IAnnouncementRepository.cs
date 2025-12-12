using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Application.Filters;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface IAnnouncementRepository
{
    Task<IEnumerable<Announcement>> GetAllAsync(AnnouncementFilter? filter = null, CancellationToken cancellationToken = default);
    Task<Announcement?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Announcement?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default);
    Task AddAsync(Announcement announcement, CancellationToken cancellationToken = default);
    Task UpdateAsync(Announcement announcement, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
