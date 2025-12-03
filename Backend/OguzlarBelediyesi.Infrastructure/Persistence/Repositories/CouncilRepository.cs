using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Infrastructure.Persistence.Database;
using OguzlarBelediyesi.Infrastructure.Persistence.Entities;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Repositories;

public sealed class CouncilRepository : ICouncilRepository
{
    private readonly OguzlarBelediyesiDbContext _context;

    public CouncilRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<CouncilDocument>> GetAllAsync()
    {
        var entities = await _context.CouncilDocuments
            .AsNoTracking()
            .OrderByDescending(d => d.Date)
            .ToListAsync();

        return entities.Select(Map).ToArray();
    }

    private static CouncilDocument Map(CouncilDocumentEntity entity)
    {
        return new CouncilDocument(
            entity.Id,
            entity.Title,
            entity.Type,
            entity.Date,
            entity.Description,
            entity.FileUrl);
    }
}
