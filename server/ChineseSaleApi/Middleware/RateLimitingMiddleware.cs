using System.Collections.Concurrent;

namespace StoreApi.Middleware;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    private static readonly ConcurrentDictionary<string, ClientRequestInfo> _clients = new();
    private readonly int _requestLimit;
    private readonly TimeSpan _timeWindow;

    public RateLimitingMiddleware(
        RequestDelegate next,
        ILogger<RateLimitingMiddleware> logger,
        IConfiguration configuration)
    {
        _next = next;
        _logger = logger;
        _requestLimit = configuration.GetValue<int>("RateLimiting:RequestLimit", 100);
        _timeWindow = TimeSpan.FromMinutes(configuration.GetValue<int>("RateLimiting:TimeWindowMinutes", 1));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientId = GetClientIdentifier(context);
        var now = DateTime.UtcNow;

        var clientInfo = _clients.GetOrAdd(clientId, new ClientRequestInfo
        {
            WindowStart = now,
            RequestCount = 0
        });

        bool isRateLimited = false;

        lock (clientInfo)
        {
            // Reset window if time has passed
            if (now - clientInfo.WindowStart > _timeWindow)
            {
                clientInfo.WindowStart = now;
                clientInfo.RequestCount = 0;
            }

            clientInfo.RequestCount++;

            if (clientInfo.RequestCount > _requestLimit)
            {
                isRateLimited = true;
            }
        }

        if (isRateLimited)
        {
            _logger.LogWarning("Rate limit exceeded for client: {ClientId}", clientId);

            context.Response.StatusCode = 429; // Too Many Requests
            context.Response.Headers.Append("Retry-After", _timeWindow.TotalSeconds.ToString());

            await context.Response.WriteAsJsonAsync(new
            {
                message = "Rate limit exceeded. Please try again later.",
                statusCode = 429,
                retryAfter = $"{_timeWindow.TotalSeconds} seconds"
            });

            return;
        }

        // Add rate limit headers to response
        context.Response.Headers.Append("X-Rate-Limit-Limit", _requestLimit.ToString());
        context.Response.Headers.Append("X-Rate-Limit-Remaining",
            Math.Max(0, _requestLimit - clientInfo.RequestCount).ToString());
        context.Response.Headers.Append("X-Rate-Limit-Reset",
            (clientInfo.WindowStart + _timeWindow).ToString("o"));

        await _next(context);
    }

    private string GetClientIdentifier(HttpContext context)
    {
        // Use IP address as client identifier
        // In production, you might want to use authenticated user ID
        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return ipAddress;
    }

    private class ClientRequestInfo
    {
        public DateTime WindowStart { get; set; }
        public int RequestCount { get; set; }
    }
}
