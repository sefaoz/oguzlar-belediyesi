using System.ComponentModel.DataAnnotations;

namespace OguzlarBelediyesi.WebAPI.Contracts.Requests;

public record ContactMessageRequest(
    [Required(ErrorMessage = "Ad Soyad zorunludur.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Ad Soyad 3-100 karakter arasında olmalıdır.")]
    string Name,
    
    [Required(ErrorMessage = "E-posta zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçersiz e-posta formatı.")]
    [StringLength(100, ErrorMessage = "E-posta en fazla 100 karakter olabilir.")]
    string Email,
    
    [Required(ErrorMessage = "Telefon numarası zorunludur.")]
    [Phone(ErrorMessage = "Geçersiz telefon numarası formatı.")]
    [StringLength(20, ErrorMessage = "Telefon numarası en fazla 20 karakter olabilir.")]
    string Phone,
    
    [Required(ErrorMessage = "Mesaj zorunludur.")]
    [StringLength(5000, MinimumLength = 10, ErrorMessage = "Mesaj 10-5000 karakter arasında olmalıdır.")]
    string Message,
    
    [Required(ErrorMessage = "KVKK onayı zorunludur.")]
    bool KvkkAccepted
);
