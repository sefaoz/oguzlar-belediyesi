using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OguzlarBelediyesi.WebAPI.Contracts.Requests;

public class UnitUpsertRequest
{
    [Required(ErrorMessage = "Başlık zorunludur.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Başlık 2-200 karakter arasında olmalıdır.")]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(5000, ErrorMessage = "İçerik en fazla 5000 karakter olabilir.")]
    public string? Content { get; set; }
    
    [Required(ErrorMessage = "İkon zorunludur.")]
    [StringLength(100, ErrorMessage = "İkon en fazla 100 karakter olabilir.")]
    public string Icon { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Slug zorunludur.")]
    [StringLength(200, ErrorMessage = "Slug en fazla 200 karakter olabilir.")]
    [RegularExpression(@"^[a-z0-9\-]+$", ErrorMessage = "Slug sadece küçük harf, rakam ve tire içerebilir.")]
    public string Slug { get; set; } = string.Empty;
    
    public string? StaffJson { get; set; }
}
