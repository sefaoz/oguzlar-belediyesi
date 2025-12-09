using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OguzlarBelediyesi.Application.Contracts.Services;
using OguzlarBelediyesi.WebAPI;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    /// <summary>
    /// Kullanıcı girişi yapar ve JWT token döner.
    /// Rate limiting: IP başına dakikada maksimum 5 deneme.
    /// </summary>
    [HttpPost("login")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authenticationService.AuthenticateAsync(request.Username, request.Password);
        return result is not null 
            ? Ok(new TokenResponse(result.Token, result.RefreshToken)) 
            : Unauthorized();
    }

    /// <summary>
    /// Süresi dolmuş token'ı yeniler.
    /// Rate limiting: IP başına dakikada maksimum 5 deneme.
    /// </summary>
    [HttpPost("refresh-token")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        var result = await _authenticationService.RefreshTokenAsync(request.Token, request.RefreshToken);
        return result is not null 
            ? Ok(new TokenResponse(result.Token, result.RefreshToken)) 
            : Unauthorized();
    }
}
