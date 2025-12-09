using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI.Filters;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/gallery")]
public sealed class GalleryController : ControllerBase
{
    private readonly IGalleryRepository _repository;
    private readonly IWebHostEnvironment _env;

    public GalleryController(IGalleryRepository repository, IWebHostEnvironment env)
    {
        _repository = repository;
        _env = env;
    }

    [HttpGet("folders")]
    //[Cache(120, "Gallery")] // Removing cache for now to see updates immediately, or handle invalidation
    public async Task<ActionResult<IEnumerable<GalleryFolder>>> GetFolders()
    {
        var folders = await _repository.GetFoldersAsync();
        return Ok(folders);
    }

    [HttpGet("folders/{folderId:guid}")]
    public async Task<ActionResult<GalleryFolder>> GetFolderById(Guid folderId)
    {
        var folder = await _repository.GetFolderByIdAsync(folderId);
        return folder is null ? NotFound() : Ok(folder);
    }

    [HttpGet("folders/slug/{slug}")]
    public async Task<ActionResult<GalleryFolder>> GetFolderBySlug(string slug)
    {
        var folder = await _repository.GetFolderBySlugAsync(slug);
        return folder is null ? NotFound() : Ok(folder);
    }

    [HttpPost("folders")]
    [Authorize]
    public async Task<IActionResult> CreateFolder([FromBody] GalleryFolderRequest request)
    {
        if (request.IsFeatured)
        {
             var folders = await _repository.GetFoldersAsync();
             if (folders.Count(f => f.IsFeatured) >= 2)
             {
                 return BadRequest("En fazla 2 galeri ana sayfada gösterilebilir.");
             }
        }

        var slug = await GenerateUniqueSlugAsync(request.Title);

        var folder = new GalleryFolder
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Slug = slug,
            Date = request.Date,
            ImageCount = 0,
            CoverImage = "", // Will be set when images are added
            IsFeatured = request.IsFeatured,
            IsActive = request.IsActive
        };

        await _repository.CreateFolderAsync(folder);
        return CreatedAtAction(nameof(GetFolderById), new { folderId = folder.Id }, folder);
    }

    [HttpPut("folders/{id:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateFolder(Guid id, [FromBody] GalleryFolderRequest request)
    {
        var existing = await _repository.GetFolderByIdAsync(id);
        if (existing is null) return NotFound();

        if (request.IsFeatured && !existing.IsFeatured)
        {
             var folders = await _repository.GetFoldersAsync();
             if (folders.Count(f => f.IsFeatured) >= 2)
             {
                 return BadRequest("En fazla 2 galeri ana sayfada gösterilebilir.");
             }
        }

        var slug = existing.Slug;
        if (existing.Title != request.Title)
        {
            slug = await GenerateUniqueSlugAsync(request.Title, id);
        }

        existing.Title = request.Title;
        existing.Slug = slug;
        existing.Date = request.Date;
        existing.IsFeatured = request.IsFeatured;
        existing.IsActive = request.IsActive;

        await _repository.UpdateFolderAsync(existing);
        return NoContent();
    }

    [HttpDelete("folders/{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteFolder(Guid id)
    {
        await _repository.DeleteFolderAsync(id);
        return NoContent();
    }

    [HttpGet("folders/{folderId:guid}/images")]
    public async Task<ActionResult<IEnumerable<GalleryImage>>> GetImagesByFolder(Guid folderId)
    {
        var images = await _repository.GetImagesByFolderAsync(folderId);
        return Ok(images);
    }

    [HttpPost("images")]
    [Authorize]
    public async Task<IActionResult> AddImage([FromForm] Guid folderId, [FromForm] string? title, [FromForm] IFormFile file)
    {
         if (file is null) return BadRequest("File is required");

         var folder = await _repository.GetFolderByIdAsync(folderId);
         if (folder is null) return NotFound("Folder not found");

         var url = await OguzlarBelediyesi.WebAPI.Helpers.ImageHelper.SaveImageAsWebPAsync(file, "uploads/gallery", _env.WebRootPath);
         
         if (string.IsNullOrEmpty(url)) return BadRequest("Failed to upload image");

         var image = new GalleryImage
         {
             Id = Guid.NewGuid(),
             FolderId = folderId,
             Url = url,
             ThumbnailUrl = url, // Using same URL for now, strictly should create thumbnail
             Title = title
         };

         await _repository.AddImageAsync(image);
         return Ok(image);
    }

    [HttpDelete("images/{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteImage(Guid id)
    {
        await _repository.DeleteImageAsync(id);
        return NoContent();
    }

    private async Task<string> GenerateUniqueSlugAsync(string title, Guid? excludeId = null)
    {
        var baseSlug = OguzlarBelediyesi.WebAPI.Helpers.SlugHelper.GenerateSlug(title);
        var slug = baseSlug;
        var counter = 2;

        while (true)
        {
             var existing = await _repository.GetFolderBySlugAsync(slug);
             // If existing is found AND it is NOT the one we are excluding (updating), then slug is taken
             if (existing != null) 
             {
                 if (excludeId.HasValue && existing.Id == excludeId.Value)
                 {
                     break; // It's the same record, so slug is fine
                 }
                 // Taken, try next
                 slug = OguzlarBelediyesi.WebAPI.Helpers.SlugHelper.AppendNumber(baseSlug, counter);
                 counter++;
             }
             else
             {
                 break; // Not found, safe to use
             }
        }

        return slug;
    }
}
