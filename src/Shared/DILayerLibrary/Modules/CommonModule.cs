using BSLayerSchool.BSInterfaces.CommonContracts;
using BSLayerSchool.BSRepositories.CommonServices;
using DataBaseServices.Core.Contracts.CommonContracts;
using DataBaseServices.Core.Services.CommonServices;
using MCCommonLayer.Interface;
using MCCommonLayer.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection.Modules
{
    internal static class CommonModule
    {
        /// <summary>
        /// Inject services for Common micro service module.
        /// </summary>
        /// <param name="services">IServiceCollection object.</param>
        /// <returns>return injected Services.</returns>

        public static IServiceCollection InjectServices(this IServiceCollection services)
        {

            //Distributed Cache Service
            services.AddScoped<IBsCurrencyContract, BsCurrencyService>();
            services.AddScoped<IBsLanguageContract, BsLanguageService>();
            services.AddScoped<IBsVendorContract, BsVendorService>();


            //Distributed Cache Service
            services.AddScoped<IMCCurrencyContract, MCCurrencyService>();
            services.AddScoped<IMCLanguageContract, MCLanguageService>();
            services.AddScoped<IMCVendorContract, MCVendorService>();
            services.AddScoped<IMCAddressContract, MCAddressService>();

            //Data Model layer for Common module
            services.AddScoped<IDataLayerCurrencyContract, DLCurrencyService>();
            services.AddScoped<IDataLayerLanguageContract, DLLanguageService>();
            services.AddScoped<IDataLayerVendorContract, DLVendorService>();
            services.AddScoped<IDataLayerAddressContract, DLAddressService>();


            return services;
        }

    }
}
