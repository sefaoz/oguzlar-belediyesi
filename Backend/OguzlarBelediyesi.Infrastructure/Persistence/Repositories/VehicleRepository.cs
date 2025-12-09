using System.Collections.Generic;
using System.Linq;
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

    public async Task<IReadOnlyList<Vehicle>> GetAllAsync()
    {
        var vehicles = await _context.Vehicles
            .AsNoTracking()
            .OrderBy(v => v.Name)
            .ToListAsync();

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
    public async Task<Vehicle?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Vehicles.FindAsync(id);
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

    public async Task AddAsync(Vehicle vehicle)
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

        await _context.Vehicles.AddAsync(entity);
    }

    public async Task UpdateAsync(Vehicle vehicle)
    {
        var entity = await _context.Vehicles.FindAsync(vehicle.Id);
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

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Vehicles.FindAsync(id);
        if (entity != null)
        {
            _context.Vehicles.Remove(entity);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
