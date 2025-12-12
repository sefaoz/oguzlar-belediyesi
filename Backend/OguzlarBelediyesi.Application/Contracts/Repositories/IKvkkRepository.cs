using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface IKvkkRepository
{
    Task<IReadOnlyList<KvkkDocument>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<KvkkDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(KvkkDocument document, CancellationToken cancellationToken = default);
    Task UpdateAsync(KvkkDocument document, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
