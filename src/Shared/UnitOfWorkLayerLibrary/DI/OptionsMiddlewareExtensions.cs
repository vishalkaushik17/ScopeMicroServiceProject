using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace UnitOfWork.DI;

public static class OptionsMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomCorsFilter(this IApplicationBuilder builder, IConfiguration configuration)
    {
        return builder.UseMiddleware<UseCustomCORSOptionsMiddleWare>();
    }
}