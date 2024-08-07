using GenericFunction.Constants.AppConfig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;
using System.Threading.RateLimiting;



namespace DependencyInjection.Modules
{
    internal static class ApiGateway
    {
        /// <summary>
        /// Inject services for Api Gateway specific micro service.
        /// </summary>
        /// <param name="services">IServiceCollection object.</param>
        /// <returns>return injected Services.</returns>

        public static IServiceCollection InjectServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var environmentName = Environment.GetEnvironmentVariable(SoftwareEnvironment.ASPNETCORE_ENVIRONMENT);
            services.AddRateLimiter(temp => temp
                .AddFixedWindowLimiter(policyName: "fixed", options =>
                {
                    options.PermitLimit = 1;
                    options.Window = TimeSpan.FromSeconds(2);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 1;
                }));
            //builder.Services.EnableCors(builder.Configuration);
            builder.Configuration.AddJsonFile("ocelot.json", optional: true, reloadOnChange: true);
            builder.Configuration.AddJsonFile($"ocelot.{environmentName}.json", optional: true, reloadOnChange: true);
            builder.Services.AddOcelot().AddPolly();

            return services;
        }

    }
}
