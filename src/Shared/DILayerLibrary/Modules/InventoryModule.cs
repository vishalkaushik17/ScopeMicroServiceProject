using BSLayerSchool.BSInterfaces.InventoryManagement;
using BSLayerSchool.BSRepositories.InventoryManagement;
using DataBaseServices.Core.Contracts.CommonContracts;
using DataBaseServices.Core.Services.CommonServices;
using MCInventoryLayer.Interface;
using MCInventoryLayer.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection.Modules
{
    internal static class InventoryModule
    {
        /// <summary>
        /// Inject services for Inventory module.
        /// </summary>
        /// <param name="services">IServiceCollection object.</param>
        /// <returns>return injected Services.</returns>

        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            services.AddScoped<IBsProductContract, BsProductService>();
            services.AddScoped<IMCProductContract, MCProductService>();
            services.AddScoped<IDataLayerProductContract, DLProductService>();
            return services;
        }

    }
}
