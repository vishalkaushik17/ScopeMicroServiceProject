using GenericFunction.Constants.AppConfig;
namespace DependancyInjection;

using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

public class UserAgentMiddleware
{
    private readonly RequestDelegate _next;

    public UserAgentMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userAgent = context.Request.Headers["User-Agent"].ToString().ToLower();

        var acceptHeader = context.Request.Headers["Accept"].ToString().ToLower();
        var acceptLanguageHeader = context.Request.Headers["Accept-Language"].ToString().ToLower();

        bool isBrowser = userAgent.Contains("mozilla") ||
                         userAgent.Contains("chrome") ||
                         userAgent.Contains("safari") ||
                         userAgent.Contains("opera") ||
                         userAgent.Contains("edge");


        var oldBrowsers = new[] {
            "explorer",  // Firefox
        };

        // Check for Internet Explorer user-agent strings
        if (userAgent.Contains("msie") || userAgent.Contains("trident"))
        {
            // Redirect to the Microsoft Edge download page
            context.Response.Redirect("https://www.microsoft.com/edge");
            return;
        }

        // Check for common browser headers
        bool hasAcceptHeader = acceptHeader.Contains("text/html") || acceptHeader.Contains("application/xhtml+xml");
        bool hasAcceptLanguageHeader = !string.IsNullOrWhiteSpace(acceptLanguageHeader);


        // Check if the user-agent contains any of the known browser strings
        if (isBrowser && hasAcceptHeader && hasAcceptLanguageHeader)
        {
            // Continue processing the request
            await _next(context);
        }
        else
        {
            if (Environment.GetEnvironmentVariable(SoftwareEnvironment.ASPNETCORE_ENVIRONMENT) == SoftwareEnvironment.Production)
            {
                // Reject the request with 403 Forbidden
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access denied. Only browsers are allowed.");

            }
            else
            {
                await _next(context);
            }
        }
    }
}
