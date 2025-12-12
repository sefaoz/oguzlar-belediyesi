using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.WebAPI.Contracts.Requests;

public record NewsRequest(
    [Required(ErrorMessage = "Başlık zorunludur.")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Başlık 5-200 karakter arasında olmalıdır.")]
    string Title,
    
    [Required(ErrorMessage = "Açıklama zorunludur.")]
    [StringLength(50000, MinimumLength = 10, ErrorMessage = "Açıklama en az 10 karakter olmalıdır.")]
    string Description,
    
    [Required(ErrorMessage = "Tarih zorunludur.")]
    DateTime Date,
    
    string? Image,
    
    IReadOnlyList<string>? Tags,
    
    IReadOnlyList<string>? Photos
);

public record AnnouncementRequest(
    [Required(ErrorMessage = "Başlık zorunludur.")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Başlık 5-200 karakter arasında olmalıdır.")]
    string Title,
    
    [Required(ErrorMessage = "Özet zorunludur.")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Özet 10-500 karakter arasında olmalıdır.")]
    string Summary,
    
    [Required(ErrorMessage = "İçerik zorunludur.")]
    [StringLength(50000, MinimumLength = 10, ErrorMessage = "İçerik en az 10 karakter olmalıdır.")]
    string Content,
    
    [Required(ErrorMessage = "Kategori zorunludur.")]
    [StringLength(100, ErrorMessage = "Kategori en fazla 100 karakter olabilir.")]
    string Category,
    
    [Required(ErrorMessage = "Tarih zorunludur.")]
    DateTime Date
);

public record EventRequest(
    [Required(ErrorMessage = "Başlık zorunludur.")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Başlık 5-200 karakter arasında olmalıdır.")]
    string Title,
    
    [Required(ErrorMessage = "Açıklama zorunludur.")]
    [StringLength(50000, MinimumLength = 10, ErrorMessage = "Açıklama en az 10 karakter olmalıdır.")]
    string Description,
    
    [Required(ErrorMessage = "Konum zorunludur.")]
    [StringLength(300, ErrorMessage = "Konum en fazla 300 karakter olabilir.")]
    string Location,
    
    [Required(ErrorMessage = "Etkinlik tarihi zorunludur.")]
    DateTime EventDate,
    
    [Required(ErrorMessage = "Etkinlik saati zorunludur.")]
    [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Geçersiz saat formatı. Örnek: 14:30")]
    string EventTime,
    
    string? Image
);

public record TenderRequest(
    [StringLength(300, MinimumLength = 5, ErrorMessage = "Başlık 5-300 karakter arasında olmalıdır.")]
    string? Title,
    
    [StringLength(50000, ErrorMessage = "Açıklama en fazla 50000 karakter olabilir.")]
    string? Description,
    
    [StringLength(100, ErrorMessage = "Kayıt numarası en fazla 100 karakter olabilir.")]
    string? RegistrationNumber,
    
    [RegularExpression("^(Open|Closed|Cancelled|Completed)$", ErrorMessage = "Geçersiz durum. Geçerli durumlar: Open, Closed, Cancelled, Completed")]
    string? Status,
    
    DateTime? TenderDate,
    
    string? DocumentsJson
);

public record SliderRequest(
    [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
    string? Title,
    
    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
    string? Description,
    
    string? ImageUrl,
    
    [Url(ErrorMessage = "Geçersiz URL formatı.")]
    string? Link,
    
    [Range(0, 100, ErrorMessage = "Sıra 0-100 arasında olmalıdır.")]
    int Order,
    
    bool IsActive
);

public class SliderFormRequest
{
    [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
    public string? Title { get; set; }
    
    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
    public string? Description { get; set; }
    
    public string? ImageUrl { get; set; }
    
    [Url(ErrorMessage = "Geçersiz URL formatı.")]
    public string? Link { get; set; }
    
    [Range(0, 100, ErrorMessage = "Sıra 0-100 arasında olmalıdır.")]
    public int Order { get; set; }
    
    public bool IsActive { get; set; }
    
    public IFormFile? File { get; set; }
}

public record PageContentRequest(
    [Required(ErrorMessage = "Key zorunludur.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Key 2-100 karakter arasında olmalıdır.")]
    [RegularExpression(@"^[a-z0-9\-]+$", ErrorMessage = "Key sadece küçük harf, rakam ve tire içerebilir.")]
    string Key,
    
    [Required(ErrorMessage = "Başlık zorunludur.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Başlık 2-200 karakter arasında olmalıdır.")]
    string Title,
    
    [StringLength(300, ErrorMessage = "Alt başlık en fazla 300 karakter olabilir.")]
    string Subtitle,
    
    IReadOnlyList<string> Paragraphs,
    
    string? ImageUrl,
    
    string? MapEmbedUrl,
    
    IReadOnlyList<ContactDetail>? ContactDetails
);
