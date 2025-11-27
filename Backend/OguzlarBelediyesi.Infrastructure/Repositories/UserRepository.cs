using Microsoft.EntityFrameworkCore;
using OguzlarBelediyesi.Application;
using OguzlarBelediyesi.Domain;
using OguzlarBelediyesi.Infrastructure.Database;

namespace OguzlarBelediyesi.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly OguzlarBelediyesiDbContext _context;

    public UserRepository(OguzlarBelediyesiDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
