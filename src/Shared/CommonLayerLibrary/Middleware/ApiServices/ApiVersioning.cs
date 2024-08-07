using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace GenericFunction.Middleware.ApiServices;

public static class ApiVersioning
{
    public static void AddApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        //services.addver .AddVersionedApiExplorer(config =>
        //{
        //    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service  
        //    // note: the specified format code will format the version as "'v'major[.minor][-status]"  
        //    config.GroupNameFormat = "'v'VVV";

        //    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat  
        //    // can also be used to control the format of the API version in route templates  
        //    config.SubstituteApiVersionInUrl = true;
        //});

    }

}