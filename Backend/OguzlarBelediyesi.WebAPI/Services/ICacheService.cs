using Microsoft.Extensions.Primitives;

namespace OguzlarBelediyesi.WebAPI.Services;

public interface ICacheService
{
    IChangeToken GetToken(string tag);
    void Invalidate(string tag);
}
