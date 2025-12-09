using OguzlarBelediyesi.Domain.Entities.Messages;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

/// <summary>
/// İletişim mesajları için repository arayüzü.
/// </summary>
public interface IContactMessageRepository
{
    Task<IReadOnlyList<ContactMessage>> GetAllAsync();
    Task<IReadOnlyList<ContactMessage>> GetByTypeAsync(string messageType);
    Task<ContactMessage?> GetByIdAsync(Guid id);
    Task<int> GetUnreadCountAsync();
    Task AddAsync(ContactMessage message);
    Task UpdateAsync(ContactMessage message);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}
