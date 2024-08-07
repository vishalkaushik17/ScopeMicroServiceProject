using IdentityLayer.Middleware.ApiServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace IdentityLayer.JwtSecurity.JWTSecurity;

public static class SwaggerJwtAuth
{
    public static void AddJttAuthToSwagger(this IServiceCollection services)
    {
        //var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        //{
        //    Name = "JWT Authentication",
        //    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        //    Scheme = "bearer",
        //    BearerFormat = "JWT",
        //    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        //    Description = "Enter a valid JWT bearer token.",
        //    Reference = new Microsoft.OpenApi.Models.OpenApiReference
        //    {
        //        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
        //        Id = JwtBearerDefaults.AuthenticationScheme
        //    }
        //};

        services.AddSwaggerGen(setup =>
        {
            // Include 'SecurityScheme' to use JWT Authentication
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,

                Description = "Please enter token!",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            };
            setup.OperationFilter<SwaggerDefaultValues>();

            setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() }
            });

        });
        //services.AddSwaggerGen(
        //    options =>
        //    {
        //        options.OperationFilter<SwaggerDefaultValues>();
        //        options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
        //        options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        //        {
        //            {securityScheme,new string[] {}}
        //        });
        //    });
    }
}
