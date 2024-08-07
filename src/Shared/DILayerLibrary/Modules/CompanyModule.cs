using BSAuthentication.BsInterface.AccountService;
using BSAuthentication.BsServices.AccountService;
using BSCodingCompany.BSInterfaces.Company;
using BSCodingCompany.BSInterfaces.DemoRequest;
using BSCodingCompany.BSServices.Company;
using BSCodingCompany.BSServices.DemoRequest;
using BSCodingCompany.BSServices.Hosting;
using BSLayerSchool.BSInterfaces.AppContracts;
using BSLayerSchool.BSInterfaces.SchoolLibraryContracts;
using BSLayerSchool.BSRepositories.AppServices;
using DataBaseServices.Core.Contracts.AppContracts;
using DataBaseServices.Core.Contracts.CodingCompany;
using DataBaseServices.Core.Contracts.CommonServices;
using DataBaseServices.Core.Contracts.UsersAndRoles;
using DataBaseServices.Core.Services.AppContracts;
using DataBaseServices.Core.Services.CodingCompany;
using DataBaseServices.Core.Services.CommonServices;
using DataBaseServices.Core.Services.UsersAndRoles;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection.Modules
{
    internal static class CompanyModule
    {
        /// <summary>
        /// Inject services for Company module..
        /// </summary>
        /// <param name="services">IServiceCollection object.</param>
        /// <returns>return injected Services.</returns>

        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            services.AddScoped<IBsSystemPreferencesContract, BsSystemPreferencesService>();
            services.AddScoped<IBsCompanyMasterContract, BsCompanyMasterService>();
            services.AddScoped<IBsApplicationHostContract, BsApplicationHostService>();
            services.AddScoped<IBsCompanyTypeContract, BsCompanyTypeService>();
            services.AddScoped<IBsAccountService, BsAccountService>();
            services.AddScoped<IBsDemoRequestContract, BsDemoRequestService>();




            services.AddScoped<IDataLayerSystemPreferencesContract, DLSystemPreferenceService>();
            services.AddScoped<IDataLayerApplicationHostContract, DLApplicationHostService>();
            services.AddScoped<IDataLayerRoleContract, DLRoleService>();
            services.AddScoped<IDataLayerUserContract, DLUserService>();
            services.AddScoped<IDataLayerNewDBService, DLCreateNewDBService>();
            services.AddScoped<IDataLayerDemoRequestContract, DLDemoRequestService>();

            services.AddScoped<IDataLayerCompanyMaster, DLCompanyMasterService>();
            services.AddScoped<IDataLayerCompanyMasterProfile, DLCompanyMasterProfileService>();
            services.AddScoped<IDataLayerCompanyMasterVsDbHost, DLCompanyMasterVsDbHostService>();
            services.AddScoped<IDataLayerCompanyType, DLCompanyTypeService>();
            services.AddScoped<IDataLayerDemoRequestContract, DLDemoRequestService>();
            return services;
        }

    }
}
