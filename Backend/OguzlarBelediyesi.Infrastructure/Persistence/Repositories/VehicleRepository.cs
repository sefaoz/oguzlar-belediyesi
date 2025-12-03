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

        return vehicles.Select(entity => new Vehicle(
            entity.Id,
            entity.Name,
            entity.Type,
            entity.Plate,
            entity.Description,
            entity.ImageUrl)).ToArray();
    }
}
