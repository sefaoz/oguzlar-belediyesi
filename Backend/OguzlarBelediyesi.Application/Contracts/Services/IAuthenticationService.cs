using System.Threading.Tasks;

namespace OguzlarBelediyesi.Application.Contracts.Services;

public interface IAuthenticationService
{
    Task<string?> AuthenticateAsync(string username, string password);
}
