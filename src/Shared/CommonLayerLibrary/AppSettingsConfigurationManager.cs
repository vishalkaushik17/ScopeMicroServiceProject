using GenericFunction.Constants.AppConfig;
using Microsoft.Extensions.Configuration;

namespace GenericFunction;

public static class AppSettingsConfigurationManager
{
    public static IConfiguration AppSetting { get; }
    static AppSettingsConfigurationManager()
    {
        var environmentName = Environment.GetEnvironmentVariable(SoftwareEnvironment.ASPNETCORE_ENVIRONMENT);

        //var builder = new ConfigurationBuilder()
        //    .SetBasePath(Directory.GetCurrentDirectory())
        //    .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
        //    .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
        //    .AddEnvironmentVariables();

        AppSetting = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
            .Build();
    }
}
