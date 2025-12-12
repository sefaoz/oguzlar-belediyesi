using System.Text.Json;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Application.Filters;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI;
using OguzlarBelediyesi.WebAPI.Contracts.Requests;
using OguzlarBelediyesi.WebAPI.Filters;
using OguzlarBelediyesi.WebAPI.Helpers;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/tenders")]
public sealed class TendersController : ControllerBase
{
    private readonly ITenderRepository _repository;
    private readonly IWebHostEnvironment _env;

    public TendersController(ITenderRepository repository, IWebHostEnvironment env)
    {
        _repository = repository;
        _env = env;
    }

    [HttpGet]
    [Cache(60, "Tenders")]
    public async Task<ActionResult<IEnumerable<Tender>>> Get([FromQuery] TenderQuery query, CancellationToken cancellationToken)
    {
        var filter = new TenderFilter(query.search, query.status);
        var tenders = await _repository.GetAllAsync(filter, cancellationToken);
        return Ok(tenders);
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<Tender>> GetBySlug(string slug, CancellationToken cancellationToken)
    {
        var tender = await _repository.GetBySlugAsync(slug, cancellationToken);
        return tender is null ? NotFound() : Ok(tender);
    }

    [HttpPost]
    [Authorize]
    [CacheInvalidate("Tenders")]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = 500 * 1024 * 1024)]
    public async Task<IActionResult> Create([FromForm] TenderRequest request, [FromForm] List<IFormFile>? documentFiles, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest("Title is required.");
        }

        var slug = await GenerateUniqueSlugAsync(request.Title, null, cancellationToken);

        var documents = new List<TenderDocument>();
        
        if (!string.IsNullOrEmpty(request.DocumentsJson))
        {
            try 
            {
                var existing = JsonSerializer.Deserialize<List<TenderDocument>>(request.DocumentsJson);
                if (existing != null) documents.AddRange(existing);
            }
            catch {}
        }

        if (documentFiles is not null && documentFiles.Count > 0)
        {
            foreach (var file in documentFiles)
            {
                var path = await SaveFileAsync(file, "uploads/tenders", cancellationToken);
                documents.Add(new TenderDocument { Title = file.FileName, Url = path });
            }
        }

        var tender = new Tender
        {
            Id = Guid.NewGuid(),
            Title = request.Title ?? string.Empty,
            Description = request.Description ?? string.Empty,
            RegistrationNumber = request.RegistrationNumber ?? string.Empty,
            Status = request.Status ?? "Open",
            TenderDate = request.TenderDate ?? DateTime.UtcNow,
            DocumentsJson = JsonSerializer.Serialize(documents),
            Slug = slug
        };

        await _repository.AddAsync(tender, cancellationToken);
        
        return CreatedAtAction(nameof(GetBySlug), new { slug = tender.Slug }, tender);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Tenders")]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = 500 * 1024 * 1024)]
    public async Task<IActionResult> Update(Guid id, [FromForm] TenderRequest request, [FromForm] List<IFormFile>? documentFiles, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return NotFound();
        }

        var slug = existing.Slug;
        if (!string.IsNullOrWhiteSpace(request.Title) && existing.Title != request.Title)
        {
            slug = await GenerateUniqueSlugAsync(request.Title, id, cancellationToken);
        }

        var documents = new List<TenderDocument>();
        
        if (!string.IsNullOrEmpty(request.DocumentsJson))
        {
             try 
            {
                var keep = JsonSerializer.Deserialize<List<TenderDocument>>(request.DocumentsJson);
                if (keep != null) documents.AddRange(keep);
            }
            catch {}
        }

        if (documentFiles is not null && documentFiles.Count > 0)
        {
            foreach (var file in documentFiles)
            {
                var path = await SaveFileAsync(file, "uploads/tenders", cancellationToken);
                documents.Add(new TenderDocument { Title = file.FileName, Url = path });
            }
        }

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            existing.Title = request.Title;
        }
        
        if (!string.IsNullOrWhiteSpace(request.Description)) existing.Description = request.Description;
        if (!string.IsNullOrWhiteSpace(request.RegistrationNumber)) existing.RegistrationNumber = request.RegistrationNumber;
        if (!string.IsNullOrWhiteSpace(request.Status)) existing.Status = request.Status;

        if (request.TenderDate.HasValue) 
        {
            existing.TenderDate = request.TenderDate.Value;
        }
        existing.DocumentsJson = JsonSerializer.Serialize(documents);
        existing.Slug = slug;
        existing.UpdateDate = DateTime.UtcNow;

        await _repository.UpdateAsync(existing, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [CacheInvalidate("Tenders")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    private async Task<string> GenerateUniqueSlugAsync(string title, Guid? excludeId, CancellationToken cancellationToken)
    {
        var baseSlug = SlugHelper.GenerateSlug(title);
        var slug = baseSlug;
        var counter = 2;

        while (await _repository.SlugExistsAsync(slug, excludeId, cancellationToken))
        {
            slug = SlugHelper.AppendNumber(baseSlug, counter);
            counter++;
        }

        return slug;
    }

    private async Task<string> SaveFileAsync(IFormFile file, string folder, CancellationToken cancellationToken)
    {
        var uploadsFolder = Path.Combine(_env.WebRootPath, folder);
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        return Path.Combine(folder, uniqueFileName).Replace("\\", "/");
    }
}

public class TenderDocument
{
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
