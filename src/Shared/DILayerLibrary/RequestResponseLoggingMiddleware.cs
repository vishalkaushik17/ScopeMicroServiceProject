namespace DependancyInjection;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

/// <summary>
/// Printing request and response.
/// </summary>
public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        // Print the request to the console
        Console.WriteLine(await FormatRequest(context.Request));

        // Copy the original response body stream
        var originalBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            // Replace the response body with the new stream
            context.Response.Body = responseBody;

            // Call the next middleware in the pipeline
            await _next(context);

            // Print the response to the console
            Console.WriteLine(await FormatResponse(context.Response));

            // Copy the contents of the new response body back to the original stream
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private async Task<string> FormatRequest(HttpRequest request)
    {
        request.EnableBuffering();
        var body = request.Body;

        // Read the body
        var buffer = new byte[Convert.ToInt32(request.ContentLength)];
        await request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length));
        var bodyAsText = System.Text.Encoding.UTF8.GetString(buffer);

        // Reset the body stream position for the next middleware
        request.Body.Position = 0;

        return $"{request.Method} {request.Scheme}://{request.Host}{request.Path} {request.QueryString} {bodyAsText}";
    }

    private async Task<string> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);
        return $"Response {response.StatusCode}: {text}";
    }
}
