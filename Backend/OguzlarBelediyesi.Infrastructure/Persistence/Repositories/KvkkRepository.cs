using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Infrastructure.Persistence.Database;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Repositories;

public sealed class KvkkRepository : IKvkkRepository
{
    private readonly OguzlarBelediyesiDbContext _context;

    public KvkkRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<KvkkDocument>> GetAllAsync()
    {
        var entities = await _context.KvkkDocuments
            .AsNoTracking()
            .OrderBy(d => d.Title)
            .ToListAsync();

        return entities.Select(entity => new KvkkDocument(entity.Id, entity.Title, entity.Type, entity.FileUrl)).ToArray();
    }
}
