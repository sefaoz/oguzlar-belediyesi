using OguzlarBelediyesi.Domain;
using System;
using System.Threading.Tasks;

namespace OguzlarBelediyesi.Application.Contracts.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
    Task SaveChangesAsync();
}
