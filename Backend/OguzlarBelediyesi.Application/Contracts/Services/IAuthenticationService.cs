using System.Threading.Tasks;

namespace OguzlarBelediyesi.Application.Contracts.Services;

public record AuthResult(string Token, string RefreshToken);

public interface IAuthenticationService
{
    Task<AuthResult?> AuthenticateAsync(string username, string password);
    Task<AuthResult?> RefreshTokenAsync(string token, string refreshToken);
}
