using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Infrastructure.Persistence.Database;
using OguzlarBelediyesi.Infrastructure.Persistence.Entities;

namespace OguzlarBelediyesi.Infrastructure.Persistence.Repositories;

public sealed class MunicipalUnitRepository : IMunicipalUnitRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly OguzlarBelediyesiDbContext _context;

    public MunicipalUnitRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<MunicipalUnit>> GetAllAsync()
    {
        var entities = await _context.MunicipalUnits
            .AsNoTracking()
            .OrderBy(u => u.Title)
            .ToListAsync();

        return entities.Select(Map).ToArray();
    }

    private static MunicipalUnit Map(MunicipalUnitEntity entity)
    {
        var staff = ParseStaff(entity.StaffJson);
        return new MunicipalUnit(
            entity.Id,
            entity.Title,
            entity.Content,
            entity.Icon,
            staff);
    }

    private static IReadOnlyList<UnitStaff> ParseStaff(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return Array.Empty<UnitStaff>();
        }

        return JsonSerializer.Deserialize<List<UnitStaff>>(json, JsonOptions) ?? new List<UnitStaff>();
    }
}
