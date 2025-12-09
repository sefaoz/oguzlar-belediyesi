using System;
using System.Collections.Generic;
using System.Linq;
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

    public async Task<IEnumerable<Slider>> GetAllAsync()
    {
        var entities = await _context.Sliders
            .AsNoTracking()
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.Order)
            .ToListAsync();

        return entities.Select(entity => Map(entity)).ToArray();
    }

    public async Task<Slider?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Sliders
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        return entity is null ? null : Map(entity);
    }

    public async Task AddAsync(Slider slider)
    {
        await _context.Sliders.AddAsync(Map(slider));
    }

    public Task UpdateAsync(Slider slider)
    {
        _context.Sliders.Update(Map(slider));
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Sliders.FindAsync(id);
        if (entity is not null)
        {
            entity.IsDeleted = true;
            entity.UpdateDate = DateTime.UtcNow;
        }
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
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
