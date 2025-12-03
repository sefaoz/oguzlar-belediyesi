using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace OguzlarBelediyesi.WebAPI.Filters;

public sealed class AspectCacheFilter : IEndpointFilter
{
    private readonly IMemoryCache _cache;

    public AspectCacheFilter(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var cacheAttribute = context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<CacheAttribute>();
        if (cacheAttribute is null)
        {
            return await next(context);
        }

        var key = GenerateCacheKey(context.HttpContext.Request);
        if (_cache.TryGetValue(key, out object? cached))
        {
            return cached;
        }

        var result = await next(context);
        _cache.Set(key, result, TimeSpan.FromSeconds(cacheAttribute.DurationSeconds));
        return result;
    }

    private static string GenerateCacheKey(HttpRequest request)
    {
        var builder = new StringBuilder();
        builder.Append(request.Method).Append(':').Append(request.Path);
        if (request.QueryString.HasValue)
        {
            builder.Append('?').Append(request.QueryString.Value);
        }
        return builder.ToString();
    }
}
