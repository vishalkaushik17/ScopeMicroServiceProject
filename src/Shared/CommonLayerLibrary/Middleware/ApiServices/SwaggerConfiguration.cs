using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace GenericFunction.Middleware.ApiServices;

public class SwaggerConfiguration
{
    public void ConfigureSwagger(WebApplication app, IApiVersionDescriptionProvider provider)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            // point 2 
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                options.DefaultModelsExpandDepth(-1);
            });
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }


        //app.UseSwaggerUI(
        //                options =>
        //                {
        //                    // build a swagger endpoint for each discovered API version
        //                    foreach (var description in provider.ApiVersionDescriptions)
        //                    {
        //                        options.DefaultModelsExpandDepth(-1); // Disable swagger schemas at bottom

        //                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
        //                            description.GroupName.ToUpperInvariant());
        //                        options.SwaggerEndpoint("../swagger/v1/swagger.json", "My API V1");
        //                    }
        //                }


        //);
    }
}
