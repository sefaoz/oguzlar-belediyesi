using System.Collections.Generic;
using System.Threading.Tasks;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface IKvkkRepository
{
    Task<IReadOnlyList<KvkkDocument>> GetAllAsync();
    Task<KvkkDocument?> GetByIdAsync(Guid id);
    Task AddAsync(KvkkDocument document);
    Task UpdateAsync(KvkkDocument document);
    Task DeleteAsync(Guid id);
    Task SaveChangesAsync();
}
