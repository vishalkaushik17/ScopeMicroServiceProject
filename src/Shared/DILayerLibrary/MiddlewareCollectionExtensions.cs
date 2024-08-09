using Asp.Versioning.ApiExplorer;
using GenericFunction.Constants.AppConfig;
using GenericFunction.GlobalHandler;
using GenericFunction.Middleware.ApiServices;
using IdentityLayer.JwtSecurity.JWTSecurity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using ModelTemplates.RequestNResponse.CommonResponse;
using Ocelot.Middleware;
using System.Reflection;
using UnitOfWork.DI;
namespace DependancyInjection;


/// <summary>
/// Middlewawre registrations.
/// </summary>
public static class MiddlewareCollectionExtensions
{

    /// <summary>
    /// Register middleware for MVC UI.
    /// </summary>
    /// <param name="app"></param>
    public static void AddCustomMiddlewareModuleWiseForMVCUI(this WebApplication app, WebApplicationBuilder builder, Assembly assemblyName)
    {
        app.UseCustomCorsFilter(builder.Configuration);

        //for getting ipv4 Ip address
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                ForwardedHeaders.XForwardedProto
        });
        app.UseHttpsRedirection();
        app.UseStaticFiles(new StaticFileOptions()
        {
            OnPrepareResponse =
                r =>
                {
                    //cache for maximum time
                    TimeSpan maxAge = new TimeSpan(7, 0, 0, 0);
                    r.Context.Response.Headers.Append("Cache-Control", "public, max-age=" + maxAge.TotalSeconds.ToString("0"));
                    //}
                }
        });
        app.UseMiddleware<RequestResponseLoggingMiddleware>();

        app.UseRouting();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseSession();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            ControllerActionEndpointConventionBuilder controllerActionEndpointConventionBuilder = endpoints.MapAreaControllerRoute(
                name: "Identity",
                areaName: "Identity",
                pattern: "Identity/{controller=Account}/{action=Login}"
            );
            ControllerActionEndpointConventionBuilder controllerActionEndpointConventionBuilder1 = endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area=Home}/{controller=Home}/{action=Index}/{id?}"
            );
        });

        app.Run();
    }


    /// <summary>
    /// Use ocelot middlewawre configuration for api gateway micro service.
    /// </summary>
    /// <param name="app"></param>
    public static void AddCustomMiddlewareModuleWiseForOcelot(this WebApplication app, Assembly assemblyName)
    {

        //for getting ipv4 Ip address
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                               ForwardedHeaders.XForwardedProto
        });


        app.UseMiddleware<UserAgentMiddleware>();

        //set minimal api to check response
        MinimalApiResponse response = new MinimalApiResponse();

        response.AssemblyName = assemblyName.GetName().Name;
        response.AssemblyVersion = assemblyName.GetName().Version;
        app.MapGet("/ping", () => response.ToString());
        app.MapGet("/", () => response.ToString());

        app.UseHttpsRedirection();
        app.UseMiddleware<RequestResponseLoggingMiddleware>();

        app.UseRouting();

        app.UseCors(AppConst.CorsPolicy);

#pragma warning disable ASP0014
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            //endpoints.MapHub<ChatHub>("/chatHub").RequireCors(AppConst.CorsPolicy);
        });


        app.UseOcelot().Wait();
        app.Run();
#pragma warning restore ASP0014
    }

    public static void AddCustomMiddlewareModuleWise(this WebApplication app, WebApplicationBuilder builder,
            Assembly assemblyName)
    {
        if (assemblyName.GetName().Name != "APIGateway")
            app.UseCustomCorsFilter(builder.Configuration);
        //enable automatic database migration.
        //MigrationManager.MigrateDatabase(app);

        //for getting ipv4 Ip address
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                               ForwardedHeaders.XForwardedProto
        });

        if (assemblyName.GetName().Name != "APIGateway")
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();


            var configSwagger = new SwaggerConfiguration();
            configSwagger.ConfigureSwagger(app, provider);

        }




        //set minimal api to check response
        MinimalApiResponse response = new MinimalApiResponse();

        response.AssemblyName = assemblyName.GetName().Name;
        response.AssemblyVersion = assemblyName.GetName().Version;
        app.MapGet("/ping", () => response.ToString());
        app.MapGet("/", () => response.ToString());

        app.UseHttpsRedirection();
        app.UseMiddleware<RequestResponseLoggingMiddleware>();

        app.UseRouting();
        if (assemblyName.GetName().Name != "APIGateway")
            app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseCors(AppConst.CorsPolicy);
        if (assemblyName.GetName().Name != "APIGateway")
            app.UseMiddleware<JwtMiddleware>();

#pragma warning disable ASP0014
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            //endpoints.MapHub<ChatHub>("/chatHub").RequireCors(AppConst.CorsPolicy);

        });

        app.Run();
#pragma warning restore ASP0014
    }

}