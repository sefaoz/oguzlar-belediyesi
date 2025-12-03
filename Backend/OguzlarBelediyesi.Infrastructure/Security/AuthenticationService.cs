using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Application.Contracts.Services;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Infrastructure.Security;

namespace OguzlarBelediyesi.Infrastructure.Security;

public sealed class JwtSettings
{
    public string Key { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int ExpirationMinutes { get; init; } = 60;
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

    public async Task<string?> AuthenticateAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user is null || !_passwordHasher.Verify(password, user.PasswordHash))
        {
            return null;
        }

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Username),
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
}
