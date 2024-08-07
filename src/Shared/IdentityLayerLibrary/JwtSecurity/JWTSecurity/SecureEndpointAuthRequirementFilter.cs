using Microsoft.OpenApi.Models;
using SharedLibrary.Services;
using SharedLibrary.Services.CustomFilters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IdentityLayer.JwtSecurity.JWTSecurity;

internal class SecureEndpointAuthRequirementFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!context.ApiDescription
                .ActionDescriptor
                .EndpointMetadata
                .OfType<CustomAuthorizeAttribute>()
                .Any())
        {
            return;
        }

        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                [new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "token" }
                }] = new List<string>()
            }
        };
    }
}
