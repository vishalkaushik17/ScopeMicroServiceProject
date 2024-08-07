using BSLayerSchool.BSInterfaces.EmployeeContracts;
using BSLayerSchool.BSRepositories.CommonServices;
using BSLayerSchool.BSRepositories.InventoryManagement;
using DataBaseServices.Core.Contracts.CommonContracts;
using DataBaseServices.Core.Contracts.EmployeeContracts;
using DataBaseServices.Core.Services.CommonServices;
using MCEmployeeLayer.Interface;
using MCEmployeeLayer.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection.Modules
{
    internal static class EmployeeModule
    {
        /// <summary>
        /// Inject services for Employee micro service.
        /// </summary>
        /// <param name="services">IServiceCollection object.</param>
        /// <returns>return injected Services.</returns>

        public static IServiceCollection InjectService(this IServiceCollection services)
        {
            //Adding Business Layer Services
            services.AddScoped<IBsBankMasterContract, BsBankService>();
            services.AddScoped<IBsDegreeContract, BsDegreeService>();
            services.AddScoped<IBsDepartmentContract, BsDepartmentService>();
            services.AddScoped<IBsDesignationMasterContract, BsDesignationService>();
            services.AddScoped<IBsEmployeeStudentParentContract, BsEmployeeStudentParentService>();
            services.AddScoped<IBsEmployeeQualificationContract, BsEmployeeQualificationService>();

            //adding common layer Di for Memory cache
            services.AddScoped<IMCDepartmentContract, MCDepartmentService>();
            services.AddScoped<IMCBankContract, MCBankService>();
            services.AddScoped<IMCDegreeContract, MCDegreeService>();
            services.AddScoped<IMCDesignationContract, MCDesignationService>();
            services.AddScoped<IMCEmployeeQualificationContract, MCEmployeeQualificationService>();
            services.AddScoped<IMCEmployeeStudentParentContract, MCEmployeeStudentParentService>();

            //adding DI for DataLayer
            services.AddScoped<IDataLayerBankContract, DLBankService>();
            services.AddScoped<IDataLayerDegreeContract, DLDegreeService>();
            services.AddScoped<IDataLayerDepartmentContract, DLDepartmentService>();
            services.AddScoped<IDataLayerDesignationContract, DLDesignationsService>();
            services.AddScoped<IDataLayerEmployeeQualificationContract, DLEmployeeQualificationService>();
            services.AddScoped<IDataLayerEmployeeStudentParentContract, DLEmployeeStudentParentService>();


            return services;
        }

    }
}
