using DependancyInjection;
using GenericFunction.Enums;
using SharedLibrary.Services.DBConfig;
namespace SchoolLibraryMicroService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //here EnumDBType is defined for which database is configured for the application.
            builder = builder.EstablishDbConnection();

            //registering dependency injection for EmployeeModule
            builder.Services.AddCustomServices(builder, EnumModuleNames.LibraryModule, typeof(Program));

            //Middleware registrations
            builder.Build().AddCustomMiddlewareModuleWise(builder, typeof(Program).Assembly);
        }
    }
}