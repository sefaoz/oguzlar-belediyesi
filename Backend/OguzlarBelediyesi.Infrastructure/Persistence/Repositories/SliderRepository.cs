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
            .OrderBy(s => s.Order)
            .ToListAsync();

        return entities.Select(entity => Map(entity)).ToArray();
    }

    public async Task<Slider?> GetByIdAsync(string id)
    {
        var entity = await _context.Sliders
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);
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

    public async Task DeleteAsync(string id)
    {
        var entity = await _context.Sliders.FindAsync(id);
        if (entity is not null)
        {
            _context.Sliders.Remove(entity);
        }
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    private static Slider Map(SliderEntity entity)
    {
        return new Slider(entity.Id, entity.Title, entity.Description, entity.ImageUrl, entity.Link, entity.Order, entity.IsActive);
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
