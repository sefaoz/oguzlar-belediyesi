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

public sealed class KvkkRepository : IKvkkRepository
{
    private readonly OguzlarBelediyesiDbContext _context;

    public KvkkRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<KvkkDocument>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.KvkkDocuments
            .AsNoTracking()
            .Where(d => !d.IsDeleted)
            .OrderBy(d => d.Title)
            .ToListAsync(cancellationToken);

        return entities.Select(entity => new KvkkDocument
        {
            Id = entity.Id,
            Title = entity.Title,
            Type = entity.Type,
            FileUrl = entity.FileUrl
        }).ToArray();
    }

    public async Task<KvkkDocument?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.KvkkDocuments.FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted, cancellationToken);
        if (entity == null) return null;

        return new KvkkDocument
        {
            Id = entity.Id,
            Title = entity.Title,
            Type = entity.Type,
            FileUrl = entity.FileUrl
        };
    }

    public async Task AddAsync(KvkkDocument document, CancellationToken cancellationToken = default)
    {
        var entity = new KvkkDocumentEntity
        {
            Id = document.Id,
            Title = document.Title,
            Type = document.Type,
            FileUrl = document.FileUrl
        };
        await _context.KvkkDocuments.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(KvkkDocument document, CancellationToken cancellationToken = default)
    {
        var entity = await _context.KvkkDocuments.FindAsync(new object[] { document.Id }, cancellationToken);
        if (entity != null)
        {
            entity.Title = document.Title;
            entity.Type = document.Type;
            entity.FileUrl = document.FileUrl;
            _context.KvkkDocuments.Update(entity);
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.KvkkDocuments.FindAsync(new object[] { id }, cancellationToken);
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
}
