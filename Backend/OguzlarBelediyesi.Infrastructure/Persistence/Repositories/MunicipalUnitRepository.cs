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
            .Where(u => !u.IsDeleted)
            .OrderBy(u => u.Title)
            .ToListAsync();

        return entities.Select(Map).ToArray();
    }

    public async Task<MunicipalUnit?> GetByIdAsync(Guid id)
    {
        var entity = await _context.MunicipalUnits
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

        return entity == null ? null : Map(entity);
    }

    public async Task CreateAsync(MunicipalUnit unit)
    {
        var entity = new MunicipalUnitEntity
        {
            Id = unit.Id,
            Title = unit.Title,
            Content = unit.Content,
            Icon = unit.Icon,
            Slug = unit.Slug,
            StaffJson = JsonSerializer.Serialize(unit.Staff ?? new List<UnitStaff>(), JsonOptions),
            IsDeleted = false
        };

        await _context.MunicipalUnits.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MunicipalUnit unit)
    {
        var entity = await _context.MunicipalUnits.FirstOrDefaultAsync(u => u.Id == unit.Id);
        if (entity == null) return;

        entity.Title = unit.Title;
        entity.Content = unit.Content;
        entity.Icon = unit.Icon;
        entity.Slug = unit.Slug;
        entity.StaffJson = JsonSerializer.Serialize(unit.Staff ?? new List<UnitStaff>(), JsonOptions);
        entity.UpdateDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.MunicipalUnits.FirstOrDefaultAsync(u => u.Id == id);
        if (entity == null) return;

        entity.IsDeleted = true;
        entity.UpdateDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
    }

    private static MunicipalUnit Map(MunicipalUnitEntity entity)
    {
        var staff = ParseStaff(entity.StaffJson);
        return new MunicipalUnit
        {
            Id = entity.Id,
            Title = entity.Title,
            Slug = entity.Slug,
            Content = entity.Content,
            Icon = entity.Icon,
            Staff = staff.ToList()
        };
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
