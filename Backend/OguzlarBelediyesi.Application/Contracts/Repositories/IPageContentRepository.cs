using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface IPageContentRepository
{
    Task<IEnumerable<PageContent>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PageContent?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task<PageContent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(PageContent pageContent, CancellationToken cancellationToken = default);
    Task UpdateAsync(PageContent pageContent, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
