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

public sealed class SliderRepository : ISliderRepository
{
    private readonly OguzlarBelediyesiDbContext _context;

    public SliderRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Slider>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.Sliders
            .AsNoTracking()
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.Order)
            .ToListAsync(cancellationToken);

        return entities.Select(entity => Map(entity)).ToArray();
    }

    public async Task<Slider?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Sliders
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted, cancellationToken);
        return entity is null ? null : Map(entity);
    }

    public async Task AddAsync(Slider slider, CancellationToken cancellationToken = default)
    {
        await _context.Sliders.AddAsync(Map(slider), cancellationToken);
    }

    public Task UpdateAsync(Slider slider, CancellationToken cancellationToken = default)
    {
        _context.Sliders.Update(Map(slider));
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Sliders.FindAsync(new object[] { id }, cancellationToken);
        if (entity is not null)
        {
            entity.IsDeleted = true;
            entity.UpdateDate = DateTime.UtcNow;
        }
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    private static Slider Map(SliderEntity entity)
    {
        return new Slider
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            ImageUrl = entity.ImageUrl,
            Link = entity.Link,
            Order = entity.Order,
            IsActive = entity.IsActive
        };
    }

    private static SliderEntity Map(Slider slider)
    {
        return new SliderEntity
        {
            Id = slider.Id,
            Title = slider.Title,
            Description = slider.Description,
            ImageUrl = slider.ImageUrl,
            Link = slider.Link,
            Order = slider.Order,
            IsActive = slider.IsActive
        };
    }
}
