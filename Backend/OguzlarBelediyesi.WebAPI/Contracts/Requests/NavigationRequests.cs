using System.ComponentModel.DataAnnotations;

namespace OguzlarBelediyesi.WebAPI.Contracts.Requests;

public record MenuRequest(
    [Required(ErrorMessage = "Başlık zorunludur.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Başlık 2-100 karakter arasında olmalıdır.")]
    string Title,
    
    [Required(ErrorMessage = "URL zorunludur.")]
    [StringLength(500, ErrorMessage = "URL en fazla 500 karakter olabilir.")]
    string Url,
    
    Guid? ParentId,
    
    [Range(0, 100, ErrorMessage = "Sıra 0-100 arasında olmalıdır.")]
    int Order,
    
    bool IsVisible,
    
    [RegularExpression("^(_self|_blank)?$", ErrorMessage = "Geçersiz target değeri. Geçerli değerler: _self, _blank")]
    string? Target
);

public record MenuOrderRequest(
    [Required(ErrorMessage = "Menü ID zorunludur.")]
    Guid Id,
    
    [Range(0, 100, ErrorMessage = "Sıra 0-100 arasında olmalıdır.")]
    int Order,
    
    Guid? ParentId
);
