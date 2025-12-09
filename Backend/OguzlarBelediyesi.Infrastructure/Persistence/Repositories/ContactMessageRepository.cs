using Microsoft.EntityFrameworkCore;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain.Entities.Messages;
using OguzlarBelediyesi.Infrastructure.Persistence.Database;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Repositories;

/// <summary>
/// İletişim mesajları için repository implementasyonu.
/// </summary>
public sealed class ContactMessageRepository : IContactMessageRepository
{
    private readonly OguzlarBelediyesiDbContext _context;

    public ContactMessageRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ContactMessage>> GetAllAsync()
    {
        return await _context.ContactMessages
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<ContactMessage>> GetByTypeAsync(string messageType)
    {
        return await _context.ContactMessages
            .Where(m => m.MessageType == messageType)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<ContactMessage?> GetByIdAsync(Guid id)
    {
        return await _context.ContactMessages.FindAsync(id);
    }

    public async Task<int> GetUnreadCountAsync()
    {
        return await _context.ContactMessages
            .Where(m => !m.IsRead)
            .CountAsync();
    }

    public async Task AddAsync(ContactMessage message)
    {
        await _context.ContactMessages.AddAsync(message);
    }

    public async Task UpdateAsync(ContactMessage message)
    {
        _context.ContactMessages.Update(message);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var message = await _context.ContactMessages.FindAsync(id);
        if (message != null)
        {
            _context.ContactMessages.Remove(message);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
