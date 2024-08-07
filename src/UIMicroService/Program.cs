using DependancyInjection;
using GenericFunction.Enums;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ScopeCoreWebApp.Components.Contracts;
using ScopeCoreWebApp.Data;
using ScopeCoreWebApp.Middleware;

namespace ScopeCoreWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            //registering dependency injection for EmployeeModule
            builder.Services.AddCustomServices(builder, EnumModuleNames.MvcUiModule, typeof(Program));

            //------------------


            //dont remove
            builder.Services.TryAddScoped<IWebApiExecutor, WebApiExecutorService>();

            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddTransient<IViewRenderService, ViewRenderService>();
            //============




            //Middleware registrations
            builder.Build().AddCustomMiddlewareModuleWiseForMVCUI(builder, typeof(Program).Assembly);

        }
    }
}