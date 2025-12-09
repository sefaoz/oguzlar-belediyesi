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
    private readonly Services.ICacheService _cacheService;

    public CacheResultFilter(IMemoryCache cache, Services.ICacheService cacheService)
    {
        _cache = cache;
        _cacheService = cacheService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cacheAttr = context.ActionDescriptor.EndpointMetadata.OfType<CacheAttribute>().FirstOrDefault();
        var invalidateAttr = context.ActionDescriptor.EndpointMetadata.OfType<CacheInvalidateAttribute>().FirstOrDefault();

        if (cacheAttr != null)
        {
            var key = GenerateCacheKey(context.HttpContext.Request);
            if (_cache.TryGetValue(key, out var cachedResult) && cachedResult is IActionResult cachedActionResult)
            {
                context.Result = cachedActionResult;
                return;
            }
        }

        var executedContext = await next();

        if (invalidateAttr != null && executedContext.Exception == null)
        {
            foreach (var tag in invalidateAttr.Tags)
            {
                _cacheService.Invalidate(tag);
            }
        }

        if (cacheAttr != null && executedContext.Result is IActionResult actionResult && executedContext.Exception == null)
        {
            var key = GenerateCacheKey(context.HttpContext.Request);
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheAttr.DurationSeconds)
            };
             
            foreach(var tag in cacheAttr.Tags)
            {
                options.AddExpirationToken(_cacheService.GetToken(tag));
            }

            _cache.Set(key, actionResult, options);
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
