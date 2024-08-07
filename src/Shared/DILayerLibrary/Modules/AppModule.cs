using DataBaseServices.Core.Contracts.CodingCompany;
using DataBaseServices.Core.Services.CodingCompany;
using DataBaseServices.LayerRepository;
using DataBaseServices.LayerRepository.Services;
using DataCacheLayer.CacheRepositories.Interfaces;
using DataCacheLayer.CacheRepositories.Repositories;
using Microsoft.Extensions.DependencyInjection;
using UnitOfWork.DI;

namespace DependencyInjection.Modules
{
    internal static class AppModule
    {
        /// <summary>
        /// Inject services for Run application.
        /// </summary>
        /// <param name="services">IServiceCollection object.</param>
        /// <returns>return injected Services.</returns>

        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            services.AddScoped<IDataLayerNewDBService, DLCreateNewDBService>();
            services.AddScoped<ICacheContract, CacheRepositoryService>();
            services.AddScoped<IDataLayerAccountService, DLAccountService>();

            //services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddTransient<ITokenService, TokenService>();


            return services;
        }

    }
}
