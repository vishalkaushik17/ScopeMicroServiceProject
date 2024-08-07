using DependancyInjection;
using GenericFunction.Enums;

namespace APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //registering dependency injection for EmployeeModule
            builder.Services.AddCustomServices(builder, EnumModuleNames.ApiGateway, typeof(Program));
            //builder.WebHost.ConfigureKestrel((context, options) =>
            //{
            //    var certPath = context.Configuration["Kestrel:Certificates:Development:Path"];
            //    var certPassword = context.Configuration["Kestrel:Certificates:Development:Password"];

            //    options.ListenAnyIP(8081, listenOptions =>
            //    {
            //        listenOptions.UseHttps(certPath, certPassword);
            //    });
            //});
            //Middleware registrations
            builder.Build().AddCustomMiddlewareModuleWiseForOcelot(typeof(Program).Assembly);
        }
    }
}