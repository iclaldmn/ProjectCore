using System.Text;

namespace WebAPI.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var start = DateTime.UtcNow;

        var user = context.User?.Identity?.Name ?? "anonymous";
        var ip = context.Connection.RemoteIpAddress?.ToString();

        context.Request.EnableBuffering();

        string body = "";

        if (context.Request.ContentLength > 0)
        {
            using var reader = new StreamReader(
                context.Request.Body,
                Encoding.UTF8,
                leaveOpen: true);

            body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        await _next(context);

        var duration = (DateTime.UtcNow - start).TotalMilliseconds;


        Serilog.Log.ForContext("RequestBody", body)
           .ForContext("User", user)
           .ForContext("IP", ip)
           .Information(
                "HTTP {Method} {Path} {StatusCode} {Duration}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                duration
           );
    }
}