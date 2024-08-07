using DependancyInjection;
using GenericFunction.Enums;
using SharedLibrary.Services.DBConfig;
namespace InventoryMicroService
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //here EnumDBType is defined for which database is configured for the application.
            builder = builder.EstablishDbConnection();

            //registering dependency injection for EmployeeModule
            builder.Services.AddCustomServices(builder, EnumModuleNames.InventoryModule, typeof(Program));

            //Middleware registrations
            builder.Build().AddCustomMiddlewareModuleWise(builder, typeof(Program).Assembly);
        }
    }
}