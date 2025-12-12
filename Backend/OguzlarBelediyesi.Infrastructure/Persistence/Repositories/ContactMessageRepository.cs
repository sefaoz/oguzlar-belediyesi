using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain.Entities.Messages;
using OguzlarBelediyesi.Infrastructure.Persistence.Database;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Repositories;

public sealed class ContactMessageRepository : IContactMessageRepository
{
    private readonly OguzlarBelediyesiDbContext _context;

    public ContactMessageRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ContactMessage>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ContactMessages
            .Where(m => !m.IsDeleted)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ContactMessage>> GetByTypeAsync(string messageType, CancellationToken cancellationToken = default)
    {
        return await _context.ContactMessages
            .Where(m => m.MessageType == messageType && !m.IsDeleted)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<ContactMessage?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ContactMessages.FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted, cancellationToken);
    }

    public async Task<int> GetUnreadCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ContactMessages
            .Where(m => !m.IsRead && !m.IsDeleted)
            .CountAsync(cancellationToken);
    }

    public async Task AddAsync(ContactMessage message, CancellationToken cancellationToken = default)
    {
        await _context.ContactMessages.AddAsync(message, cancellationToken);
    }

    public Task UpdateAsync(ContactMessage message, CancellationToken cancellationToken = default)
    {
        _context.ContactMessages.Update(message);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var message = await _context.ContactMessages.FindAsync(new object[] { id }, cancellationToken);
        if (message != null)
        {
            message.IsDeleted = true;
            message.UpdateDate = DateTime.UtcNow;
        }
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
