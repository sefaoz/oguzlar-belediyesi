using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface INewsRepository
{
    Task<IEnumerable<NewsItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<NewsItem?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<NewsItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default);
    Task AddAsync(NewsItem newsItem, CancellationToken cancellationToken = default);
    Task UpdateAsync(NewsItem newsItem, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task IncrementViewCountAsync(string slug, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
