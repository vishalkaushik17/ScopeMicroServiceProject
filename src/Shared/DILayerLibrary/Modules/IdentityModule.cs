using BSAuthentication.BsInterface;
using BSAuthentication.BsInterface.AccountService;
using BSAuthentication.BsServices;
using BSAuthentication.BsServices.AccountService;
using DataBaseServices.Core.Contracts.CommonServices;
using DataBaseServices.Core.Services.CommonServices;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection.Modules
{
    internal static class IdentityModule
    {
        /// <summary>
        /// Inject services for Identity module.
        /// </summary>
        /// <param name="services">IServiceCollection object.</param>
        /// <returns>return injected Services.</returns>
        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            services.AddScoped<IBsDbInitializerContract, BsDbInitializerService>();
            services.AddScoped<IBsAccountService, BsAccountService>();
            services.AddScoped<IDataLayerApplicationHostContract, DLApplicationHostService>();

            return services;
        }

    }
}
