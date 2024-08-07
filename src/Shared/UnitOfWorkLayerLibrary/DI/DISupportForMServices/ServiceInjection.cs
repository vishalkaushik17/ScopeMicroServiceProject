//using DataBaseServices.Core.Contracts.AppContracts;
//using DataBaseServices.Core.Contracts.CodingCompany;
//using DataBaseServices.Core.Contracts.CommonContracts;
//using DataBaseServices.Core.Contracts.CommonServices;
//using DataBaseServices.Core.Contracts.EmployeeContracts;
//using DataBaseServices.Core.Contracts.SchoolLibraryContracts;
//using DataBaseServices.Core.Contracts.UsersAndRoles;
//using DataBaseServices.Core.Services.AppContracts;
//using DataBaseServices.Core.Services.CodingCompany;
//using DataBaseServices.Core.Services.CommonServices;
//using DataBaseServices.Core.Services.SchoolLibraryService;
//using DataBaseServices.Core.Services.UsersAndRoles;
//using DataBaseServices.LayerRepository;
//using DataBaseServices.LayerRepository.Services;
//using DataCacheLayer.CacheRepositories.Interfaces;
//using DataCacheLayer.CacheRepositories.Repositories;
//using GenericFunction.GlobalService.EmailService.Contracts;
//using GenericFunction.GlobalService.EmailService.Services;
//using GenericFunction.ResultObject;
//using Microsoft.Extensions.DependencyInjection;

//namespace UnitOfWork.DI.DISupportForMServices
//{
//    //Here it will map DataLayer interface with respective Data layer Service

//    public static class ServiceInjection
//    {
//        /// <summary>
//        /// Adding common dependencies to All Micro services.
//        /// </summary>
//        /// <param name="services">IServiceCollection object</param>
//        /// <returns>IServiceCollection object</returns>
//        public static IServiceCollection AddCommonModuleDiServicesUof(this IServiceCollection services)
//        {
//            services.AddScoped<IDataLayerNewDBService, DLCreateNewDBService>();
//            services.AddScoped<ICacheContract, CacheRepositoryService>();
//            services.AddScoped<IDataLayerAccountService, DLAccountService>();
//            services.AddScoped<IEmailService, EmailService>();
//            //services.AddScoped<IEncryptionService, EncryptionService>();
//            services.AddScoped<IJwtUtils, JwtUtils>();
//            services.AddTransient<ITokenService, TokenService>();
//            services.AddScoped<ITrace, TraceRepository>();


//            //Data model service for auth module
//            services.AddScoped<IDataLayerSystemPreferencesContract, DLSystemPreferenceService>();
//            services.AddScoped<IDataLayerRoleContract, DLRoleService>();
//            services.AddScoped<IDataLayerUserContract, DLUserService>();

//            //Data Model layer for Common module
//            services.AddScoped<IDataLayerCurrencyContract, DLCurrencyService>();
//            services.AddScoped<IDataLayerLanguageContract, DLLanguageService>();
//            services.AddScoped<IDataLayerVendorContract, DLVendorService>();
//            services.AddScoped<IDataLayerAddressContract, DLAddressService>();

//            return services;
//        }

//        /// <summary>
//        /// Adding library data layer dependencies to Library Micro service module
//        /// </summary>
//        /// <param name="services">IServiceCollection object</param>
//        /// <returns>IServiceCollection object</returns>
//        public static IServiceCollection AddDataLayerServicesForLibraryMsUof(this IServiceCollection services)

//        {
//            //Library module DI registration for data operation
//            services.AddScoped<IDataLayerLibraryAuthorContract, DLLibraryAuthorService>();
//            services.AddScoped<IDataLayerLibraryBookCollectionContract, DLLibraryBookCollectionService>();
//            services.AddScoped<IDataLayerLibraryBookMasterContract, DLLibraryBookMasterService>();
//            services.AddScoped<IDataLayerLibraryBookshelfContract, DLLibraryBookshelfService>();
//            services.AddScoped<IDataLayerSchoolLibraryHallContract, DLLibraryHallService>();
//            services.AddScoped<IDataLayerLibraryMediaTypeContract, DLLibraryMediaTypeService>();
//            services.AddScoped<IDataLayerLibraryRackContract, DLLibraryRackService>();
//            services.AddScoped<IDataLayerLibraryRoomContract, DLLibraryRoomService>();
//            services.AddScoped<IDataLayerLibrarySectionContract, DLLibrarySectionService>();

//            return services;
//        }

//        /// <summary>
//        /// DataLayer for Coding Company
//        /// </summary>
//        /// <param name="services">IServiceCollection object</param>
//        /// <returns>IServiceCollection object</returns>
//        public static IServiceCollection AddDataLayerServicesForCodingCompanyMsUof(this IServiceCollection services)

//        {
//            services.AddScoped<IDataLayerAccountService, DLAccountService>();
//            services.AddScoped<IDataLayerApplicationHostContract, DLApplicationHostService>();
//            services.AddScoped<IDataLayerDemoRequestContract, DLDemoRequestService>();
//            services.AddScoped<IDataLayerCompanyType, DLCompanyTypeService>();
//            services.AddScoped<IDataLayerCompanyMaster, DLCompanyMasterService>();
//            services.AddScoped<IDataLayerCompanyMasterProfile, DLCompanyMasterProfileService>();
//            services.AddScoped<IDataLayerCompanyMasterVsDbHost, DLCompanyMasterVsDbHostService>();

//            return services;
//        }

//        /// <summary>
//        /// DataLayer Service for Authentication
//        /// </summary>
//        /// <param name="services">IServiceCollection object</param>
//        /// <returns>IServiceCollection object</returns>
//        public static IServiceCollection AddDataLayerServicesForAuthenticationMsUof(this IServiceCollection services)

//        {
//            services.AddScoped<IDataLayerApplicationHostContract, DLApplicationHostService>();
//            services.AddScoped<IDataLayerNewDBService, DLCreateNewDBService>();

//            services.AddScoped<IDataLayerCompanyMaster, DLCompanyMasterService>();
//            services.AddScoped<IDataLayerCompanyMasterProfile, DLCompanyMasterProfileService>();
//            services.AddScoped<IDataLayerCompanyMasterVsDbHost, DLCompanyMasterVsDbHostService>();
//            services.AddScoped<IDataLayerCompanyType, DLCompanyTypeService>();
//            services.AddScoped<IDataLayerDemoRequestContract, DLDemoRequestService>();
//            return services;
//        }

//        /// <summary>
//        /// DataLayer service for inventory module.
//        /// </summary>
//        /// <param name="services">IServiceCollection object</param>
//        /// <returns>IServiceCollection object</returns>
//        public static IServiceCollection AddDataLayerServicesForInventoryModule(this IServiceCollection services)
//        {
//            services.AddScoped<IDataLayerCurrencyContract, DLCurrencyService>();
//            services.AddScoped<IDataLayerLanguageContract, DLLanguageService>();
//            services.AddScoped<IDataLayerVendorContract, DLVendorService>();

//            services.AddScoped<IDataLayerProductContract, DLProductService>();

//            return services;
//        }

//        /// <summary>
//        /// DataLayer Service for Employee Module.
//        /// </summary>
//        /// <param name="services">IServiceCollection object</param>
//        /// <returns>IServiceCollection object</returns>
//        public static IServiceCollection AddDLServiceForEmployeeModule(this IServiceCollection services)

//        {
//            services.AddScoped<IDataLayerEmployeeStudentParentContract, DLEmployeeStudentParentService>();
//            services.AddScoped<IDataLayerDepartmentContract, DLDepartmentService>();
//            services.AddScoped<IDataLayerDesignationContract, DLDesignationsService>();

//            return services;
//        }


//    }

//}
