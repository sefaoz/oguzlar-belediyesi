using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OguzlarBelediyesi.Application.Contracts.Services;
using OguzlarBelediyesi.WebAPI;
using OguzlarBelediyesi.WebAPI.Contracts.Requests;

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

    [HttpPost("login")]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authenticationService.AuthenticateAsync(request.Username, request.Password);
        return result is not null 
            ? Ok(new TokenResponse(result.Token, result.RefreshToken)) 
            : Unauthorized();
    }

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
