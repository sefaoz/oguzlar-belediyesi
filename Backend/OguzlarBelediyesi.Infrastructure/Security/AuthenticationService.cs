using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Application.Contracts.Services;
using OguzlarBelediyesi.Domain;

namespace OguzlarBelediyesi.Infrastructure.Security;

public sealed class JwtSettings
{
    public string Key { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int ExpirationMinutes { get; init; } = 60;
    public int RefreshTokenExpirationDays { get; init; } = 7;
}

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;
    private readonly JwtSettings _jwtSettings;

    public AuthenticationService(
        IUserRepository userRepository,
        PasswordHasher passwordHasher,
        IOptions<JwtSettings> jwtOptions)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<AuthResult?> AuthenticateAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user is null || !_passwordHasher.Verify(password, user.PasswordHash))
        {
            return null;
        }

        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays > 0 ? _jwtSettings.RefreshTokenExpirationDays : 7);
        
        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();

        return new AuthResult(accessToken, refreshToken);
    }

    public async Task<AuthResult?> RefreshTokenAsync(string token, string refreshToken)
    {
        var principal = GetPrincipalFromExpiredToken(token);
        if (principal is null) return null;

        var username = principal.Identity?.Name;
        if (string.IsNullOrEmpty(username)) return null;

        var user = await _userRepository.GetByUsernameAsync(username);
        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return null;
        }

        var newAccessToken = GenerateAccessToken(user);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays > 0 ? _jwtSettings.RefreshTokenExpirationDays : 7);

        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();

        return new AuthResult(newAccessToken, newRefreshToken);
    }

    private string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Username),
            new(ClaimTypes.Name, user.Username),
            new("role", user.Role),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
