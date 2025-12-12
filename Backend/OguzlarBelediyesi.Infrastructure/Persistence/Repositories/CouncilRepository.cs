using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

    public async Task<IReadOnlyList<CouncilDocument>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.CouncilDocuments
            .AsNoTracking()
            .Where(d => !d.IsDeleted)
            .OrderByDescending(d => d.Date)
            .ToListAsync(cancellationToken);

        return entities.Select(Map).ToArray();
    }

    public async Task<CouncilDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.CouncilDocuments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity is null ? null : Map(entity);
    }

    public async Task AddAsync(CouncilDocument document, CancellationToken cancellationToken = default)
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
        await _context.CouncilDocuments.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(CouncilDocument document, CancellationToken cancellationToken = default)
    {
        var entity = await _context.CouncilDocuments.FirstOrDefaultAsync(x => x.Id == document.Id, cancellationToken);
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

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.CouncilDocuments.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.UpdateDate = DateTime.UtcNow;
        }
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
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
