using System.ComponentModel.DataAnnotations;

namespace OguzlarBelediyesi.WebAPI.Contracts.Requests;

public record LoginRequest(
    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3-50 karakter arasında olmalıdır.")]
    string Username,
    
    [Required(ErrorMessage = "Şifre zorunludur.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
    string Password
);

public record RefreshTokenRequest(
    [Required(ErrorMessage = "Token zorunludur.")]
    string Token,
    
    [Required(ErrorMessage = "Refresh token zorunludur.")]
    string RefreshToken
);

public record UserRequest(
    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3-50 karakter arasında olmalıdır.")]
    string Username,
    
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
    string? Password,
    
    [Required(ErrorMessage = "Rol zorunludur.")]
    [RegularExpression("^(Admin|Editor|Viewer)$", ErrorMessage = "Geçersiz rol. Geçerli roller: Admin, Editor, Viewer")]
    string Role
);
