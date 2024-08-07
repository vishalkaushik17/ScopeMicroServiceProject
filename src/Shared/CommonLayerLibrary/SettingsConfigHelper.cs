using GenericFunction.Constants.AppConfig;
using Microsoft.Extensions.Configuration;

namespace GenericFunction;
public class SettingsConfigHelper
{
    static IConfigurationRoot configuration;
    private static SettingsConfigHelper _appSettings;
    public string AppSettingValue { get; set; }

    public static string AppSetting(string section, string Key)
    {
        _appSettings = GetCurrentSettings(section, Key);
        return _appSettings.AppSettingValue;
    }

    public static T AppSetting<T>(string section, string Key)
    {
        _appSettings = GetCurrentSettings(section, Key);
        return (T)Convert.ChangeType(_appSettings.AppSettingValue, typeof(T));
    }

    public SettingsConfigHelper(IConfiguration config, string Key)
    {
        if (!string.IsNullOrWhiteSpace(Key))
            this.AppSettingValue = config.GetValue<string>(Key);
    }
    //public SettingsConfigHelper(IConfiguration config)
    //{
    //        this.AppSettingValue = config.Get<string[]>();
    //}

    // Get a valued stored in the appsettings.
    // Pass in a key like TestArea:TestKey to get TestValue
    public static SettingsConfigHelper GetCurrentSettings(string section, string Key)
    {
        ReadAppSettings();
        //if (string.IsNullOrWhiteSpace(Key))
        //    return new SettingsConfigHelper(configuration.GetSection(section).Get<string[]>());

        return new SettingsConfigHelper(configuration.GetSection(section), Key);

    }
    public static void ReadAppSettings()
    {
        var environmentName = Environment.GetEnvironmentVariable(SoftwareEnvironment.ASPNETCORE_ENVIRONMENT);

        configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables().Build();

    }

    public static T GetOptions<T>()
      where T : class, new()
    {
        ReadAppSettings();
        return configuration.GetSection(typeof(T).Name).Get<T>() ?? new T();
    }
}

//public static class ConfigurationExtensionsAppJson
//{
//    static IConfigurationRoot configuration;
//    static ConfigurationExtensionsAppJson()
//    {
//        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

//        var builder = new ConfigurationBuilder()
//            .SetBasePath(Directory.GetCurrentDirectory())
//            .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
//            .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
//            .AddEnvironmentVariables();

//        configuration = builder.Build();
//    }

//    public static T GetOptions<T>()
//        where T : class, new()
//        => configuration.GetSection(typeof(T).Name).Get<T>() ?? new T();
//}
