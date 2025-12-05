using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace OguzlarBelediyesi.WebAPI.Filters;

public sealed class CacheResultFilter : IAsyncActionFilter
{
    private readonly IMemoryCache _cache;

    public CacheResultFilter(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cacheAttribute = context.ActionDescriptor.EndpointMetadata.OfType<CacheAttribute>().FirstOrDefault();
        if (cacheAttribute is null)
        {
            await next();
            return;
        }

        var key = GenerateCacheKey(context.HttpContext.Request);
        if (_cache.TryGetValue(key, out var cachedResult) && cachedResult is IActionResult cachedActionResult)
        {
            context.Result = cachedActionResult;
            return;
        }

        var executedContext = await next();
        if (executedContext.Result is IActionResult actionResult)
        {
            _cache.Set(key, actionResult, TimeSpan.FromSeconds(cacheAttribute.DurationSeconds));
        }
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
