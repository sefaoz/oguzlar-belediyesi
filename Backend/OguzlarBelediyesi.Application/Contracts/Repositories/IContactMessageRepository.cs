using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain.Entities.Messages;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface IContactMessageRepository
{
    Task<IReadOnlyList<ContactMessage>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ContactMessage>> GetByTypeAsync(string messageType, CancellationToken cancellationToken = default);
    Task<ContactMessage?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(CancellationToken cancellationToken = default);
    Task AddAsync(ContactMessage message, CancellationToken cancellationToken = default);
    Task UpdateAsync(ContactMessage message, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
