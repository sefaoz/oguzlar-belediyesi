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

public sealed class MenuRepository : IMenuRepository
{
    private readonly OguzlarBelediyesiDbContext _context;

    public MenuRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MenuItem>> GetAllAsync()
    {
        var entities = await _context.MenuItems
            .AsNoTracking()
            .OrderBy(e => e.Order)
            .ToListAsync();

        return entities.Select(Map);
    }

    public async Task<MenuItem?> GetByIdAsync(Guid id)
    {
        var entity = await _context.MenuItems
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        return entity is null ? null : Map(entity);
    }

    public async Task AddAsync(MenuItem menuItem)
    {
        var entity = Map(menuItem);
        await _context.MenuItems.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MenuItem menuItem)
    {
        var entity = await _context.MenuItems.FirstOrDefaultAsync(m => m.Id == menuItem.Id);
        if (entity is null)
        {
            return;
        }

        entity.Title = menuItem.Title;
        entity.Url = menuItem.Url;
        entity.ParentId = menuItem.ParentId;
        entity.Order = menuItem.Order;
        entity.IsVisible = menuItem.IsVisible;
        entity.Target = menuItem.Target;

        _context.MenuItems.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.MenuItems.FirstOrDefaultAsync(m => m.Id == id);
        if (entity is not null)
        {
            _context.MenuItems.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateOrderAsync(IEnumerable<MenuItem> orderedItems)
    {
        if (orderedItems is null)
        {
            return;
        }

        var ids = orderedItems.Select(m => m.Id).ToHashSet();
        var entities = await _context.MenuItems.Where(e => ids.Contains(e.Id)).ToListAsync();

        foreach (var entity in entities)
        {
            var updated = orderedItems.FirstOrDefault(m => m.Id == entity.Id);
            if (updated is null)
            {
                continue;
            }

            entity.Order = updated.Order;
            entity.ParentId = updated.ParentId;
        }

        _context.MenuItems.UpdateRange(entities);
        await _context.SaveChangesAsync();
    }

    private static MenuItemEntity Map(MenuItem menuItem)
    {
        return new MenuItemEntity
        {
            Id = menuItem.Id,
            Title = menuItem.Title,
            Url = menuItem.Url,
            ParentId = menuItem.ParentId,
            Order = menuItem.Order,
            IsVisible = menuItem.IsVisible,
            Target = menuItem.Target
        };
    }

    private static MenuItem Map(MenuItemEntity entity)
    {
        return new MenuItem(
            entity.Id,
            entity.Title,
            entity.Url,
            entity.ParentId,
            entity.Order,
            entity.IsVisible,
            entity.Target);
    }
}
