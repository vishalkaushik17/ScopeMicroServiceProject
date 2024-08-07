using DependencyInjection.Modules;//.EmployeeModule;
using GenericFunction.Constants.AppConfig;
using GenericFunction.Enums;
using GenericFunction.GlobalHandler;
using GenericFunction.GlobalService.EmailService.Contracts;
using GenericFunction.GlobalService.EmailService.Services;
using GenericFunction.Helpers;
using GenericFunction.Middleware.ApiServices;
using GenericFunction.ResultObject;
using IdentityLayer.JwtSecurity.JWTSecurity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using ModelTemplates.MappingProfiles;
using SharedLibrary.Services.ModelStateValidation;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using UnitOfWork.DI;
//using static DependencyInjection.Modules;//.LibraryModule;
namespace DependancyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Custom service registration for modules.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    /// <param name="moduleNames"></param>
    /// <returns></returns>
    public static IServiceCollection AddCustomServices(this IServiceCollection services, WebApplicationBuilder builder,
        EnumModuleNames moduleNames, Type typeofProgram)
    {
        ConfigurationManager configuration = builder.Configuration;

        string? environmentName = Environment.GetEnvironmentVariable(SoftwareEnvironment.ASPNETCORE_ENVIRONMENT);
        configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        configuration.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);

        //configuring program type, which microservice Program file is getting executed.
        builder.Services.AddAutoMapper(typeofProgram);

        switch (moduleNames)
        {
            case EnumModuleNames.MvcUiModule:
                MvcUiModule.InjectServices(services);
                //CompanyModule.InjectServices(services);
                //AppModule.InjectServices(services);
                services.InjectCommonServices(builder);
                break;
            case EnumModuleNames.IdentityModule:
                IdentityModule.InjectServices(services);
                CompanyModule.InjectServices(services);
                AppModule.InjectServices(services);
                services.InjectCommonServices(builder);
                break;
            case EnumModuleNames.ApiGateway:
                ApiGateway.InjectServices(services, builder);
                services.SetCurrentCulture();
                services.EnableCORS(builder.Configuration);

                break;

            case EnumModuleNames.CommonModule:
                IdentityModule.InjectServices(services);
                CommonModule.InjectServices(services);
                CompanyModule.InjectServices(services);
                AppModule.InjectServices(services);
                services.InjectCommonServices(builder);
                break;
            case EnumModuleNames.CompanyMasterModule:
                CompanyModule.InjectServices(services);
                CommonModule.InjectServices(services);
                IdentityModule.InjectServices(services);
                AppModule.InjectServices(services);
                services.InjectCommonServices(builder);
                break;
            case EnumModuleNames.EmployeeModule:
                IdentityModule.InjectServices(services);
                CompanyModule.InjectServices(services);
                EmployeeModule.InjectService(services);
                AppModule.InjectServices(services);
                services.InjectCommonServices(builder);
                break;
            case EnumModuleNames.InventoryModule:
                IdentityModule.InjectServices(services);
                CompanyModule.InjectServices(services);
                InventoryModule.InjectServices(services);
                AppModule.InjectServices(services);
                services.InjectCommonServices(builder);
                break;
            case EnumModuleNames.LibraryModule:
                IdentityModule.InjectServices(services);
                LibraryModule.InjectServices(services);
                CompanyModule.InjectServices(services);
                AppModule.InjectServices(services);
                services.InjectCommonServices(builder);
                break;
            default:
                IdentityModule.InjectServices(services);
                services.InjectCommonServices(builder);
                break;
        }

        return services;
    }

    /// <summary>
    /// Inject common services, which are used in all micro services.
    /// </summary>
    /// <param name="services">IServiceCollection object.</param>
    /// <returns>return injected Services.</returns>

    private static IServiceCollection InjectCommonServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        // Injecting services generate response in compress format.
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });
        services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });
        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Optimal;
        });
        services.TryAddScoped<ExceptionHandlingMiddleware>();


        services.AddJttAuthToSwagger();


        //to set custom model state behaviour of project
        services.Configure<ApiBehaviorOptions>(options
            => options.SuppressModelStateInvalidFilter = true);



        //set api version
        ApiVersioning.AddApiVersioning(services);

        services.AddMapper();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>,
                                    ConfigureSwaggerOptions>();
        services.AddHttpContextAccessor();


        services.TryAddScoped<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITrace, TraceRepository>();

        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<ITrace, TraceRepository>();





        //for global level authorization and model state validation
        services.AddControllers(options =>
        {
            options.Filters.Add(new ModelStateValidationAttribute());
        }
        ).ConfigureApiBehaviorOptions(options =>
        {
            //options.SuppressModelStateInvalidFilter = true;
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var modelState = actionContext.ModelState.Values;
                return new BadRequestObjectResult(modelState);
            };
        });

        services.SetMailConfiguration(builder.Configuration);
        services.ConfigureJsonBehaviour();
        services.SetCurrentCulture();
        services.EnableCORS(builder.Configuration);

        return services;
    }

    /// <summary>
    /// Set current culture with hi-IN and en-IN
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection SetCurrentCulture(this IServiceCollection services)
    {
        services.Configure<RequestLocalizationOptions>(
            options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                        new CultureInfo("hi-IN"),
                        new CultureInfo("en-IN"),
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "en-IN", uiCulture: "en-IN");
                options.DefaultRequestCulture.Culture.NumberFormat.CurrencySymbol = "&#8377;";
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;


            });

        return services;
    }


    /// <summary>
    /// Enable CORS for micro services.
    /// </summary>
    /// <param name="services">IServiceCollection object</param>
    /// <param name="configuration">IConfiguration object</param>
    /// <returns>IServiceCollection object</returns>
    private static IServiceCollection EnableCORS(this IServiceCollection services,
        IConfiguration configuration)

    {
        string[]? hosts = configuration.GetSection("AllowOrigins").Get<string[]>();
        services.AddCors(options =>
        {
            options.AddPolicy(name: AppConst.CorsPolicy,
                corsBuilder =>
                {
                    if (hosts != null)
                        corsBuilder.WithOrigins(hosts)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                });
        });

        return services;
    }

    /// <summary>
    /// Setup Mail Configuration from appseetings:MailConfiguration.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    private static IServiceCollection SetMailConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MailConfiguration>(configuration.GetSection("MailConfiguration"));
        return services;
    }

    /// <summary>
    /// Configure JSON behavior for response.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection ConfigureJsonBehaviour(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.WriteIndented = true; // For formatted JSON output
        });

        return services;
    }
}
