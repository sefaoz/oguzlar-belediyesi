using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace OguzlarBelediyesi.WebAPI.Filters;

public sealed class ActionLogFilter : IAsyncActionFilter
{
    private readonly ILogger<ActionLogFilter> _logger;

    public ActionLogFilter(ILogger<ActionLogFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;
        var endpointName = context.ActionDescriptor.DisplayName ?? request.Path;
        var username = context.HttpContext.User?.Identity?.Name ?? "Anonymous";
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("Starting {Endpoint} {Method} by {Username}", endpointName, request.Method, username);

        try
        {
            var executedContext = await next();
            stopwatch.Stop();
            _logger.LogInformation("Completed {Endpoint} {Method} in {DurationMs}ms by {Username}", endpointName, request.Method, stopwatch.ElapsedMilliseconds, username);
        }
        catch (Exception e)
        {
            stopwatch.Stop();
            _logger.LogError(e, "Failed {Endpoint} {Method} after {DurationMs}ms by {Username}", endpointName, request.Method, stopwatch.ElapsedMilliseconds, username);
            throw;
        }
    }
}
