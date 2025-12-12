using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OguzlarBelediyesi.WebAPI.Contracts.Requests;

public record GalleryFolderRequest(
    [Required(ErrorMessage = "Başlık zorunludur.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Başlık 2-200 karakter arasında olmalıdır.")]
    string Title,
    
    [Required(ErrorMessage = "Tarih zorunludur.")]
    string Date,
    
    bool IsFeatured,
    
    bool IsActive
);

public class GalleryFolderFormRequest
{
    [Required(ErrorMessage = "Başlık zorunludur.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Başlık 2-200 karakter arasında olmalıdır.")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Tarih zorunludur.")]
    public string Date { get; set; } = string.Empty;
    
    public bool IsFeatured { get; set; }
    
    public bool IsActive { get; set; }
    
    public IFormFile? CoverImage { get; set; }
}

public record GalleryImageRequest(
    [Required(ErrorMessage = "Klasör ID zorunludur.")]
    Guid FolderId,
    
    [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
    string? Title
);

public record VehicleRequest(
    [Required(ErrorMessage = "Araç adı zorunludur.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Araç adı 2-100 karakter arasında olmalıdır.")]
    string Name,
    
    [Required(ErrorMessage = "Araç tipi zorunludur.")]
    [StringLength(50, ErrorMessage = "Araç tipi en fazla 50 karakter olabilir.")]
    string Type,
    
    [Required(ErrorMessage = "Plaka zorunludur.")]
    [StringLength(20, ErrorMessage = "Plaka en fazla 20 karakter olabilir.")]
    string Plate,
    
    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
    string Description,
    
    string? ImageUrl
);
