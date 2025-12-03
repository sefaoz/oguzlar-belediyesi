using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace OguzlarBelediyesi.WebAPI.Filters;

public sealed class AspectLogFilter : IEndpointFilter
{
    private readonly ILogger<AspectLogFilter> _logger;

    public AspectLogFilter(ILogger<AspectLogFilter> logger)
    {
        _logger = logger;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.HttpContext.Request;
        var path = request.Path;
        var method = request.Method;
        var endpointName = context.HttpContext.GetEndpoint()?.DisplayName ?? path.ToString();
        var username = context.HttpContext.User?.Identity?.Name ?? "Anonymous";
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("Starting {Endpoint} {Method} by {Username}", endpointName, method, username);

        try
        {
            var result = await next(context);
            stopwatch.Stop();
            _logger.LogInformation("Completed {Endpoint} {Method} in {DurationMs}ms by {Username}", endpointName, method, stopwatch.ElapsedMilliseconds, username);
            return result;
        }
        catch (Exception e)
        {
            stopwatch.Stop();
            _logger.LogError(e, "Failed {Endpoint} {Method} after {DurationMs}ms by {Username}", endpointName, method, stopwatch.ElapsedMilliseconds, username);
            throw;
        }
    }
}
