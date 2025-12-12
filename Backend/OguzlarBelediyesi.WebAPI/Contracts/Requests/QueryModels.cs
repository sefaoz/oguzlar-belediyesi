using System.ComponentModel.DataAnnotations;

namespace OguzlarBelediyesi.WebAPI.Contracts.Requests;

public record AnnouncementQuery(
    [StringLength(100, ErrorMessage = "Arama terimi en fazla 100 karakter olabilir.")]
    string? search,
    
    DateTime? from,
    
    DateTime? to
);

public record EventQuery(
    [StringLength(100, ErrorMessage = "Arama terimi en fazla 100 karakter olabilir.")]
    string? search,
    
    bool upcomingOnly = false
);

public record TenderQuery(
    [StringLength(100, ErrorMessage = "Arama terimi en fazla 100 karakter olabilir.")]
    string? search,
    
    [RegularExpression("^(Open|Closed|Cancelled|Completed)?$", ErrorMessage = "Geçersiz durum filtresi.")]
    string? status
);
