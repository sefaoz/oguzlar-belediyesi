using System.Collections.Concurrent;
using Microsoft.Extensions.Primitives;

namespace OguzlarBelediyesi.WebAPI.Services;

public class CacheService : ICacheService
{
    private readonly ConcurrentDictionary<string, CancellationTokenSource> _tokens = new();

    public IChangeToken GetToken(string tag)
    {
        var cts = _tokens.GetOrAdd(tag, _ => new CancellationTokenSource());
        if (cts.IsCancellationRequested)
        {
             _tokens.TryRemove(tag, out _);
             cts = _tokens.GetOrAdd(tag, _ => new CancellationTokenSource());
        }
        return new CancellationChangeToken(cts.Token);
    }

    public void Invalidate(string tag)
    {
        if (_tokens.TryRemove(tag, out var cts))
        {
            cts.Cancel();
            cts.Dispose();
        }
    }
}
