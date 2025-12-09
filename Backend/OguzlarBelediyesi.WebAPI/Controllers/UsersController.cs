using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OguzlarBelediyesi.Application.Contracts.Repositories;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Infrastructure.Security;

namespace OguzlarBelediyesi.WebAPI.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public sealed class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;

    public UsersController(IUserRepository userRepository, PasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserRequest request)
    {
        // Check if username already exists
        if (await _userRepository.GetByUsernameAsync(request.Username) != null)
        {
            return BadRequest("Username already exists.");
        }
        
        if (string.IsNullOrEmpty(request.Password))
        {
            return BadRequest("Password is required.");
        }

        var user = new User
        {
            Username = request.Username,
            PasswordHash = _passwordHasher.Hash(request.Password),
            Role = request.Role
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UserRequest request)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        user.Username = request.Username;
        user.Role = request.Role;

        if (!string.IsNullOrEmpty(request.Password))
        {
            user.PasswordHash = _passwordHasher.Hash(request.Password);
        }

        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();

        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        await _userRepository.DeleteAsync(user);
        await _userRepository.SaveChangesAsync();

        return NoContent();
    }
}
