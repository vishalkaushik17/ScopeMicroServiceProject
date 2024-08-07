using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace ModelTemplates.MappingProfiles;
/// <summary>
/// AutomapperProfiler is set in program.cs file to mapping dtos to entity and reverse.
/// No need to modify this file
/// Scope of this file is Singleton.
/// </summary>
public static class AutoMapperProfiler
{
    public static void AddMapper(this IServiceCollection services)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfile());
        });

        var mapper = config.CreateMapper();

        services.AddSingleton(mapper);
    }
}

