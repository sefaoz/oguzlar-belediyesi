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

    public async Task<CouncilDocument?> GetByIdAsync(Guid id)
    {
        var entity = await _context.CouncilDocuments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity is null ? null : Map(entity);
    }

    public async Task AddAsync(CouncilDocument document)
    {
        var entity = new CouncilDocumentEntity
        {
            Id = document.Id,
            Title = document.Title,
            Type = document.Type,
            Date = document.Date,
            Description = document.Description,
            FileUrl = document.FileUrl
        };
        await _context.CouncilDocuments.AddAsync(entity);
    }

    public async Task UpdateAsync(CouncilDocument document)
    {
        var entity = await _context.CouncilDocuments.FirstOrDefaultAsync(x => x.Id == document.Id);
        if (entity != null)
        {
            entity.Title = document.Title;
            entity.Type = document.Type;
            entity.Date = document.Date;
            entity.Description = document.Description;
            entity.FileUrl = document.FileUrl;
            
            _context.CouncilDocuments.Update(entity);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.CouncilDocuments.FirstOrDefaultAsync(x => x.Id == id);
        if (entity != null)
        {
            _context.CouncilDocuments.Remove(entity);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    private static CouncilDocument Map(CouncilDocumentEntity entity)
    {
        return new CouncilDocument
        {
            Id = entity.Id,
            Title = entity.Title,
            Type = entity.Type,
            Date = entity.Date,
            Description = entity.Description,
            FileUrl = entity.FileUrl
        };
    }
}
