using System.Collections.Concurrent;

namespace SuperERP.API.Middleware;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly ConcurrentDictionary<string, (DateTime, int)> _requests = new();
    private const int MaxRequests = 100;
    private static readonly TimeSpan TimeWindow = TimeSpan.FromMinutes(1);

    public RateLimitMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var now = DateTime.UtcNow;

        if (_requests.TryGetValue(ip, out var data))
        {
            if (now - data.Item1 < TimeWindow)
            {
                if (data.Item2 >= MaxRequests)
                {
                    context.Response.StatusCode = 429;
                    await context.Response.WriteAsJsonAsync(new { error = "Too many requests" });
                    return;
                }
                _requests[ip] = (data.Item1, data.Item2 + 1);
            }
            else
            {
                _requests[ip] = (now, 1);
            }
        }
        else
        {
            _requests[ip] = (now, 1);
        }

        await _next(context);
    }
}
