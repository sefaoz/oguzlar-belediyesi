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

public sealed class VehicleRepository : IVehicleRepository
{
    private readonly OguzlarBelediyesiDbContext _context;

    public VehicleRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var vehicles = await _context.Vehicles
            .AsNoTracking()
            .Where(v => !v.IsDeleted)
            .OrderBy(v => v.Name)
            .ToListAsync(cancellationToken);

        return vehicles.Select(entity => new Vehicle
        {
            Id = entity.Id,
            Name = entity.Name,
            Type = entity.Type,
            Plate = entity.Plate,
            Description = entity.Description,
            ImageUrl = entity.ImageUrl
        }).ToArray();
    }

    public async Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted, cancellationToken);
        if (entity == null) return null;

        return new Vehicle
        {
            Id = entity.Id,
            Name = entity.Name,
            Type = entity.Type,
            Plate = entity.Plate,
            Description = entity.Description,
            ImageUrl = entity.ImageUrl
        };
    }

    public async Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        var entity = new VehicleEntity
        {
            Id = vehicle.Id,
            Name = vehicle.Name,
            Type = vehicle.Type,
            Plate = vehicle.Plate,
            Description = vehicle.Description,
            ImageUrl = vehicle.ImageUrl
        };

        await _context.Vehicles.AddAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Vehicles.FindAsync(new object[] { vehicle.Id }, cancellationToken);
        if (entity != null)
        {
            entity.Name = vehicle.Name;
            entity.Type = vehicle.Type;
            entity.Plate = vehicle.Plate;
            entity.Description = vehicle.Description;
            entity.ImageUrl = vehicle.ImageUrl;
            
            _context.Vehicles.Update(entity);
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Vehicles.FindAsync(new object[] { id }, cancellationToken);
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
