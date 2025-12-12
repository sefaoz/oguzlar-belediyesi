using System.ComponentModel.DataAnnotations;

namespace OguzlarBelediyesi.WebAPI.Contracts.Requests;

public record SiteSettingRequest(
    [Required(ErrorMessage = "Key zorunludur.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Key 2-100 karakter arasında olmalıdır.")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Key sadece harf, rakam ve alt çizgi içerebilir.")]
    string Key,
    
    [Required(ErrorMessage = "Değer zorunludur.")]
    [StringLength(5000, ErrorMessage = "Değer en fazla 5000 karakter olabilir.")]
    string Value,
    
    [Required(ErrorMessage = "Grup key zorunludur.")]
    [StringLength(100, ErrorMessage = "Grup key en fazla 100 karakter olabilir.")]
    string GroupKey,
    
    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
    string? Description,
    
    [Range(0, 1000, ErrorMessage = "Sıra 0-1000 arasında olmalıdır.")]
    int Order
);
