using GenericFunction;
using GenericFunction.GlobalService.EmailService.Contracts;
using GenericFunction.GlobalService.EmailService.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SharedLibrary.Services;
using System.Net;

namespace DependencyInjection.Modules
{
    internal static class MvcUiModule
    {
        /// <summary>
        /// Inject services for MVC UI Application module.
        /// </summary>
        /// <param name="services">IServiceCollection object.</param>
        /// <returns>return injected Services.</returns>
        public static IServiceCollection InjectServices(this IServiceCollection services)
        {


            services.AddScoped<IEmailService, EmailService>();

            services.AddControllersWithViews();
            services.AddMvcCore().AddRazorViewEngine();
            services.AddRazorPages();

            services.AddHttpContextAccessor();


            //messaging service registration
            services.TryAddScoped<IStateManagement, StateManagement>();
            var baseUrl = AppSettingsConfigurationManager.AppSetting.GetValue<string>("ApiBaseUrl");
            var apiTimeout = AppSettingsConfigurationManager.AppSetting.GetValue<int>("ApiTimeout");

            services.AddHttpClient("AuthApi", client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = new TimeSpan(apiTimeout);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });





            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Home/Home/AccessDenied");
                options.Cookie.Name = "Cookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(40);
                options.LoginPath = new PathString("/Identity/Account/Login");
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(40);//We set Time here 
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            services.AddAntiforgery(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
                options.HttpsPort = 443;
            });

            return services;
        }

    }
}
