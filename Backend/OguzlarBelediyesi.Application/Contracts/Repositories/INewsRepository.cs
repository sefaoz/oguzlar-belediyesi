using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface INewsRepository
{
    Task<IEnumerable<NewsItem>> GetAllAsync();
    Task<NewsItem?> GetBySlugAsync(string slug);
    Task<NewsItem?> GetByIdAsync(Guid id);
    Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null);
    Task AddAsync(NewsItem newsItem);
    Task UpdateAsync(NewsItem newsItem);
    Task DeleteAsync(Guid id);
    Task IncrementViewCountAsync(string slug);
    Task SaveChangesAsync();
}
