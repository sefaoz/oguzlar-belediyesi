using Microsoft.AspNetCore.Mvc;
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

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var token = await _authenticationService.AuthenticateAsync(request.Username, request.Password);
        return token is not null ? Ok(new { token }) : Unauthorized();
    }
}
