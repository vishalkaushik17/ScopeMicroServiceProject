using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ModelTemplates.Core.JWT;
using static GenericFunction.CommonMessages;
namespace IdentityLayer.JwtSecurity.JWTSecurity;

public static class AddJwtTokenServicesExtensions
{
    public static void AddJwtTokenServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add JWT Settings
        var bindJwtSettings = new JwtSettings();
        ConfigurationBinder.Bind(configuration, (string)JsonWebTokenKeys, (object)bindJwtSettings);
        services.AddSingleton(bindJwtSettings);
        var section = configuration.GetSection("JsonWebTokenKeys:ValidAudiences");
        var auds = section.Value.Split(",").ToList();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            //options.Authority
            options.SaveToken = true;
            options.RequireHttpsMetadata = true;
            if (bindJwtSettings.IssuerSigningKey != null)
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,// bindJwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey =
                       new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes((string)bindJwtSettings.IssuerSigningKey)),


                    ValidateIssuer = true,//bindJwtSettings.ValidateIssuer,
                    ValidIssuer = bindJwtSettings.ValidIssuer,
                    ValidateAudience = true,//bindJwtSettings.ValidateAudience,
                    ValidAudiences = Enumerable.ToList<string>(bindJwtSettings?.ValidAudience?.Split(",")),
                    ValidAudience = bindJwtSettings?.ValidAudience,
                    RequireExpirationTime = bindJwtSettings.RequireExpirationTime,
                    ValidateLifetime = true,//bindJwtSettings.RequireExpirationTime,
                    ClockSkew = TimeSpan.FromDays(1),


                };

            }
        });
    }
}