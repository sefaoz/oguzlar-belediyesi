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

public sealed class MenuRepository : IMenuRepository
{
    private readonly OguzlarBelediyesiDbContext _context;

    public MenuRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.MenuItems
            .AsNoTracking()
            .Where(e => !e.IsDeleted)
            .OrderBy(e => e.Order)
            .ToListAsync(cancellationToken);

        return entities.Select(Map);
    }

    public async Task<MenuItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.MenuItems
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted, cancellationToken);

        return entity is null ? null : Map(entity);
    }

    public async Task AddAsync(MenuItem menuItem, CancellationToken cancellationToken = default)
    {
        var entity = Map(menuItem);
        await _context.MenuItems.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(MenuItem menuItem, CancellationToken cancellationToken = default)
    {
        var entity = await _context.MenuItems.FirstOrDefaultAsync(m => m.Id == menuItem.Id, cancellationToken);
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
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.MenuItems.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        if (entity is not null)
        {
            entity.IsDeleted = true;
            entity.UpdateDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task UpdateOrderAsync(IEnumerable<MenuItem> orderedItems, CancellationToken cancellationToken = default)
    {
        if (orderedItems is null)
        {
            return;
        }

        var ids = orderedItems.Select(m => m.Id).ToHashSet();
        var entities = await _context.MenuItems.Where(e => ids.Contains(e.Id)).ToListAsync(cancellationToken);

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
        await _context.SaveChangesAsync(cancellationToken);
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
        return new MenuItem
        {
            Id = entity.Id,
            Title = entity.Title,
            Url = entity.Url,
            ParentId = entity.ParentId,
            Order = entity.Order,
            IsVisible = entity.IsVisible,
            Target = entity.Target
        };
    }
}
