using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI.Contracts.Requests;
using OguzlarBelediyesi.WebAPI.Filters;
using OguzlarBelediyesi.WebAPI.Helpers;
using System.Text.Json;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/units")]
public sealed class UnitsController : ControllerBase
{
    private readonly IMunicipalUnitRepository _repository;
    private readonly IWebHostEnvironment _env;

    public UnitsController(IMunicipalUnitRepository repository, IWebHostEnvironment env)
    {
        _repository = repository;
        _env = env;
    }

    [HttpGet]
    [Cache(120, "Units")]
    public async Task<ActionResult<IReadOnlyList<MunicipalUnit>>> GetAll(CancellationToken cancellationToken)
    {
        var units = await _repository.GetAllAsync(cancellationToken);
        return Ok(units);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MunicipalUnit>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var unit = await _repository.GetByIdAsync(id, cancellationToken);
        return unit == null ? NotFound() : Ok(unit);
    }

    [HttpPost]
    [Authorize]
    [CacheInvalidate("Units")]
    public async Task<IActionResult> Create([FromForm] UnitUpsertRequest request, CancellationToken cancellationToken)
    {
        var unitId = Guid.NewGuid();
        var slug = string.IsNullOrEmpty(request.Slug) ? request.Title.ToLower().Replace(" ", "-") : request.Slug;

        var staffList = new List<UnitStaff>();
        if (!string.IsNullOrEmpty(request.StaffJson))
        {
            try 
            {
                staffList = System.Text.Json.JsonSerializer.Deserialize<List<UnitStaff>>(request.StaffJson, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<UnitStaff>();
            }
            catch
            {
            }
        }
        
        var updatedStaffList = new List<UnitStaff>();
        for (int i = 0; i < staffList.Count; i++)
        {
            var staff = staffList[i];
            var imageFile = Request.Form.Files[$"staffImage_{i}"];
            string imageUrl = staff.ImageUrl ?? "";

            if (imageFile != null)
            {
                 imageUrl = await ImageHelper.SaveImageAsWebPAsync(imageFile, "uploads/units/staff", _env.WebRootPath);
            }
            
            updatedStaffList.Add(new UnitStaff(staff.Name, staff.Title, imageUrl)); 
        }

        var unit = new MunicipalUnit
        {
            Id = unitId,
            Title = request.Title,
            Content = request.Content,
            Icon = request.Icon,
            Slug = slug,
            Staff = updatedStaffList
        };

        await _repository.CreateAsync(unit, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = unit.Id }, unit);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Units")]
    public async Task<IActionResult> Update(Guid id, [FromForm] UnitUpsertRequest request, CancellationToken cancellationToken)
    {
        var staffList = new List<UnitStaff>();
        if (!string.IsNullOrEmpty(request.StaffJson))
        {
             try 
            {
                staffList = System.Text.Json.JsonSerializer.Deserialize<List<UnitStaff>>(request.StaffJson, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<UnitStaff>();
            }
            catch {}
        }

        var updatedStaffList = new List<UnitStaff>();
        for (int i = 0; i < staffList.Count; i++)
        {
            var staff = staffList[i];
            var imageFile = Request.Form.Files[$"staffImage_{i}"];
            string imageUrl = staff.ImageUrl ?? "";

            if (imageFile != null)
            {
                 imageUrl = await ImageHelper.SaveImageAsWebPAsync(imageFile, "uploads/units/staff", _env.WebRootPath);
            }
            
            updatedStaffList.Add(new UnitStaff(staff.Name, staff.Title, imageUrl)); 
        }

        var unit = new MunicipalUnit
        {
            Id = id,
            Title = request.Title,
            Content = request.Content,
            Icon = request.Icon,
            Slug = request.Slug,
            Staff = updatedStaffList
        };

        await _repository.UpdateAsync(unit, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Units")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
