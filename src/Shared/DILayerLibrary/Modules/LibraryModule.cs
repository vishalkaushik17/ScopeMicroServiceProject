using BSLayerSchool.BSInterfaces.SchoolLibraryContracts;
using BSLayerSchool.BSRepositories.CommonServices;
using BSLayerSchool.BSRepositories.SchoolLibrary;
using DataBaseServices.Core.Contracts.SchoolLibraryContracts;
using DataBaseServices.Core.Services.SchoolLibraryService;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection.Modules
{
    internal static class LibraryModule
    {
        /// <summary>
        /// Inject services for Library Module.
        /// </summary>
        /// <param name="services">IServiceCollection object.</param>
        /// <returns>return injected Services.</returns>

        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            services.AddScoped<IBsLibraryAuthorContract, BsLibraryAuthorService>();
            services.AddScoped<IBsLibraryBookCollectionContract, BsLibraryBookCollectionService>();
            services.AddScoped<IBsSchoolLibraryHallContract, BsSchoolLibraryHallService>();
            services.AddScoped<IBsLibraryBookMasterContract, BsLibraryBookMasterService>();
            services.AddScoped<IBsLibraryBookshelfContract, BsLibraryBookshelfService>();
            services.AddScoped<IBsLibraryMediaTypeContract, BsLibraryMediaTypeService>();
            services.AddScoped<IBsLibraryRackContract, BsLibraryRackService>();
            services.AddScoped<IBsLibrarySectionContract, BsLibrarySectionService>();
            services.AddScoped<IBsLibraryRoomContract, BsLibraryRoomService>();

            //Library module DI registration for data operation
            services.AddScoped<IDataLayerLibraryAuthorContract, DLLibraryAuthorService>();
            services.AddScoped<IDataLayerLibraryBookCollectionContract, DLLibraryBookCollectionService>();
            services.AddScoped<IDataLayerLibraryBookMasterContract, DLLibraryBookMasterService>();
            services.AddScoped<IDataLayerLibraryBookshelfContract, DLLibraryBookshelfService>();
            services.AddScoped<IDataLayerSchoolLibraryHallContract, DLLibraryHallService>();
            services.AddScoped<IDataLayerLibraryMediaTypeContract, DLLibraryMediaTypeService>();
            services.AddScoped<IDataLayerLibraryRackContract, DLLibraryRackService>();
            services.AddScoped<IDataLayerLibraryRoomContract, DLLibraryRoomService>();
            services.AddScoped<IDataLayerLibrarySectionContract, DLLibrarySectionService>();
            return services;
        }
    }
}
