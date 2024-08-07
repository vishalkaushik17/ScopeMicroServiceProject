using DBOperationsLayer.Data.Context;
using GenericFunction;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace SharedLibrary.Services.Initializer;

public static class MigrationManager
{
    /// <summary>
    /// it will migrate the database on authentication micro service.
    /// </summary>
    /// <param name="webApp"></param>
    /// <returns></returns>
    /// 
    //need to work here
    public static WebApplication MigrateDatabase(this WebApplication webApp)
    {
        using (var scope = webApp.Services.CreateScope())
        {
            using (var appContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
            {
                try
                {
                    appContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    //when all server get down we are getting exception here
                    //need to work here
                    ex.SendExceptionMailAsync().GetAwaiter().GetResult();
                    //Log errors or do anything you think it's needed
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }
        }
        return webApp;
    }
}