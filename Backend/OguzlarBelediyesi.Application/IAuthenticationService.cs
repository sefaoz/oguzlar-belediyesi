using System.Threading.Tasks;

namespace OguzlarBelediyesi.Application;

public interface IAuthenticationService
{
    Task<string?> AuthenticateAsync(string username, string password);
}
