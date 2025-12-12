using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.WebAPI.Contracts.Requests;
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
    public async Task<ActionResult<IEnumerable<GalleryFolder>>> GetFolders(CancellationToken cancellationToken)
    {
        var folders = await _repository.GetFoldersAsync(cancellationToken);
        return Ok(folders);
    }

    [HttpGet("folders/{folderId:guid}")]
    public async Task<ActionResult<GalleryFolder>> GetFolderById(Guid folderId, CancellationToken cancellationToken)
    {
        var folder = await _repository.GetFolderByIdAsync(folderId, cancellationToken);
        return folder is null ? NotFound() : Ok(folder);
    }

    [HttpGet("folders/slug/{slug}")]
    public async Task<ActionResult<GalleryFolder>> GetFolderBySlug(string slug, CancellationToken cancellationToken)
    {
        var folder = await _repository.GetFolderBySlugAsync(slug, cancellationToken);
        return folder is null ? NotFound() : Ok(folder);
    }

    [HttpPost("folders")]
    [Authorize]
    public async Task<IActionResult> CreateFolder([FromForm] GalleryFolderFormRequest request, CancellationToken cancellationToken)
    {
        if (request.IsFeatured)
        {
             var folders = await _repository.GetFoldersAsync(cancellationToken);
             if (folders.Count(f => f.IsFeatured) >= 2)
             {
                 return BadRequest("En fazla 2 galeri ana sayfada gösterilebilir.");
             }
        }

        var slug = await GenerateUniqueSlugAsync(request.Title, null, cancellationToken);
        string coverImageUrl = "";

        if (request.CoverImage != null)
        {
            coverImageUrl = await OguzlarBelediyesi.WebAPI.Helpers.ImageHelper.SaveImageAsWebPAsync(request.CoverImage, "uploads/gallery", _env.WebRootPath);
        }

        var folder = new GalleryFolder
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Slug = slug,
            Date = request.Date,
            ImageCount = 0,
            CoverImage = coverImageUrl,
            IsFeatured = request.IsFeatured,
            IsActive = request.IsActive
        };

        await _repository.CreateFolderAsync(folder, cancellationToken);
        return CreatedAtAction(nameof(GetFolderById), new { folderId = folder.Id }, folder);
    }

    [HttpPut("folders/{id:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateFolder(Guid id, [FromForm] GalleryFolderFormRequest request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetFolderByIdAsync(id, cancellationToken);
        if (existing is null) return NotFound();

        if (request.IsFeatured && !existing.IsFeatured)
        {
             var folders = await _repository.GetFoldersAsync(cancellationToken);
             if (folders.Count(f => f.IsFeatured) >= 2)
             {
                 return BadRequest("En fazla 2 galeri ana sayfada gösterilebilir.");
             }
        }

        var slug = existing.Slug;
        if (existing.Title != request.Title)
        {
            slug = await GenerateUniqueSlugAsync(request.Title, id, cancellationToken);
        }

        if (request.CoverImage != null)
        {
            existing.CoverImage = await OguzlarBelediyesi.WebAPI.Helpers.ImageHelper.SaveImageAsWebPAsync(request.CoverImage, "uploads/gallery", _env.WebRootPath);
        }

        existing.Title = request.Title;
        existing.Slug = slug;
        existing.Date = request.Date;
        existing.IsFeatured = request.IsFeatured;
        existing.IsActive = request.IsActive;

        await _repository.UpdateFolderAsync(existing, cancellationToken);
        return NoContent();
    }

    [HttpDelete("folders/{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteFolder(Guid id, CancellationToken cancellationToken)
    {
        await _repository.DeleteFolderAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("folders/{folderId:guid}/images")]
    public async Task<ActionResult<IEnumerable<GalleryImage>>> GetImagesByFolder(Guid folderId, CancellationToken cancellationToken)
    {
        var images = await _repository.GetImagesByFolderAsync(folderId, cancellationToken);
        return Ok(images);
    }

    [HttpPost("images")]
    [Authorize]
    public async Task<IActionResult> AddImage([FromForm] Guid folderId, [FromForm] string? title, [FromForm] IFormFile file, CancellationToken cancellationToken)
    {
         if (file is null) return BadRequest("File is required");

         var folder = await _repository.GetFolderByIdAsync(folderId, cancellationToken);
         if (folder is null) return NotFound("Folder not found");

         var url = await OguzlarBelediyesi.WebAPI.Helpers.ImageHelper.SaveImageAsWebPAsync(file, "uploads/gallery", _env.WebRootPath);
         
         if (string.IsNullOrEmpty(url)) return BadRequest("Failed to upload image");

         var image = new GalleryImage
         {
             Id = Guid.NewGuid(),
             FolderId = folderId,
             Url = url,
             ThumbnailUrl = url,
             Title = title
         };

         await _repository.AddImageAsync(image, cancellationToken);
         return Ok(image);
    }

    [HttpDelete("images/{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteImage(Guid id, CancellationToken cancellationToken)
    {
        await _repository.DeleteImageAsync(id, cancellationToken);
        return NoContent();
    }

    private async Task<string> GenerateUniqueSlugAsync(string title, Guid? excludeId, CancellationToken cancellationToken)
    {
        var baseSlug = OguzlarBelediyesi.WebAPI.Helpers.SlugHelper.GenerateSlug(title);
        var slug = baseSlug;
        var counter = 2;

        while (true)
        {
             var existing = await _repository.GetFolderBySlugAsync(slug, cancellationToken);
             if (existing != null) 
             {
                 if (excludeId.HasValue && existing.Id == excludeId.Value)
                 {
                     break;
                 }
                 slug = OguzlarBelediyesi.WebAPI.Helpers.SlugHelper.AppendNumber(baseSlug, counter);
                 counter++;
             }
             else
             {
                 break;
             }
        }

        return slug;
    }
}
