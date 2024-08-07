using GenericFunction;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ModelTemplates.RequestNResponse.Accounts;
using static ModelTemplates.RequestNResponse.Accounts.SessionExtensions;
using static GenericFunction.CommonMessages;
using GenericFunction.Enums;
using System.Diagnostics;
namespace ScopeCoreWebApp.Methods;


/// <summary>
/// This class is used for MVC applicationly.
/// </summary>
public class AuthorizeByRoleAttribute : Attribute, IAuthorizationFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly bool _isTracingRequired;
    private string Roles { get; set; }
    public AuthorizeByRoleAttribute(params string[] roles)
    {
        Roles = String.Join(",", roles);
    }
    //public AuthorizeByRoleAttribute(IHttpContextAccessor httpContextAccessor)
    //{
    //    this._httpContextAccessor = httpContextAccessor;
    //    _isTracingRequired = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("IsTraceRequired");
    //}
    //public AuthorizeByRoleAttribute()
    //{
        
    //}
    public void OnAuthorization(AuthorizationFilterContext filterContext)
    {
        try
        {
            //  _trace.TraceMe(nameof(OnAuthorizationInvoke), _isTracingRequired, OperationStart.ToCss(), "Setting session".ToCss());

            var username = filterContext.HttpContext.GetUserName(); //Session.GetObjectFromJson<ApplicationUserDtoModel>("AuthSessionObject").UserName;
            //var username = filterContext.HttpContext.Session.GetObjectFromJson<ApplicationUserDtoModel>("AuthSessionObject").UserName;
            var token = filterContext.HttpContext.GetToken();// Session.GetObjectFromJson<ApplicationUserDtoModel>("AuthSessionObject").JwtToken;
            //var account = (LoginModel)filterContext.HttpContext.Items["UserRepository"];

            if (token != null && !string.IsNullOrWhiteSpace(username))
            {
                var roles = filterContext.HttpContext.GetUserRoles(); //.Session.GetObjectFromJson<ApplicationUserDtoModel>("AuthSessionObject").Roles;
                //var roles = filterContext.HttpContext.Session.GetObjectFromJson<List<string>>("Roles").ToList();
                //if (IsValidToken(token))
                //{
                bool matched = false;
                var rolesArray = Roles?.Split(",");
                foreach (var role in rolesArray)
                {
                    foreach (var userrole in roles)
                    {
                        if (role == userrole)
                        {
                      //      _trace.TraceMe(nameof(OnAuthorizationInvoke), _isTracingRequired, "Role matched!".Information());
                            matched = true;
                            break;
                        }

                    }
                    if (matched)
                        break;
                }

                if ((!matched && rolesArray[0] == "") || matched)
                {
                    //filterContext.HttpContext.Response.Headers.Add("authToken", token);
                    //filterContext.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");
                    //filterContext.HttpContext.Session.SetString("AuthStatus", "Authorized");
                    //filterContext.HttpContext.Response.Headers.Add("storeAccessibility", "Authorized");

                    filterContext.HttpContext.Response.Headers.Add("authToken", token);
                    filterContext.HttpContext.Response.Headers.Add("AuthStatus", nameof(AuthStatus.Authorized));
                    //filterContext.HttpContext.Response.Headers.Add("storeAccessibility", "Authorized");

                    //filterContext.HttpContext.Session.SetObjectAsJson("AuthStatus", AuthStatus.Authorized);
                    //filterContext.HttpContext.Session.SetObjectAsJson("IsSignIn", true);
                   // filterContext.HttpContext.Session.SetString("authToken", token);

                    return;
                }
                filterContext.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                //}
            }

            // }


            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                { "area", "Home" },
                { "controller", "Home" },
                { "action", "AccessDenied" },
                { "returnUrl" ,filterContext.HttpContext.Request.Path.Value }
                    //area=  "Identity",
                    //controller = "Login",
                    //action = "Login",
                    //returnUrl = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped)

                });
           // _trace.TraceMe(nameof(OnAuthorizationInvoke), _isTracingRequired, OperationEnd.ToCss(), "Setting session end!".ToCss());
        }
        catch (Exception ex)
        {
          //  _trace.TraceMe(nameof(OnAuthorizationInvoke), _isTracingRequired, OperationEnd.ToCss(), ex.Message.Information());
            filterContext.Result = new RedirectToRouteResult(
                                    new RouteValueDictionary
                                    {
                                                { "area", "Identity" },
                                                { "controller", "Account" },
                                                { "action", "Login" },
                                                { "returnUrl" ,filterContext.HttpContext.Request.Path.Value }


                                    });
        }
        // OnAuthorizationInvoke(filterContext, _httpContextAccessor);
    }
    /// <summary>  
    /// This will Authorize Username  
    /// </summary>  
    /// <returns></returns>  
    //public void OnAuthorizationInvoke(AuthorizationFilterContext filterContext, IHttpContextAccessor httpContextAccessor)
    //{
    //    //if (filterContext.HttpContext.Request.Path != null)
    //    //{
    //    ITrace _trace = new TraceRepository(httpContextAccessor);
    //    try
    //    {
    //        _trace.TraceMe(nameof(OnAuthorizationInvoke), _isTracingRequired, OperationStart.ToCss(), "Setting session".ToCss());
            
    //        var username = filterContext.HttpContext.Session.GetObjectFromJson<ApplicationUserDtoModel>("AuthSessionObject").UserName;
    //        var token = filterContext.HttpContext.Session.GetObjectFromJson<ApplicationUserDtoModel>("AuthSessionObject").JwtToken;
    //        //var account = (LoginModel)filterContext.HttpContext.Items["UserRepository"];

    //        if (token != null && !string.IsNullOrWhiteSpace(username))
    //        {
    //            var roles = filterContext.HttpContext.Session.GetObjectFromJson<ApplicationUserDtoModel>("AuthSessionObject").Roles;
    //            //var roles = filterContext.HttpContext.Session.GetObjectFromJson<List<string>>("Roles").ToList();
    //            //if (IsValidToken(token))
    //            //{
    //            bool matched = false;
    //            var rolesArray = Roles.Split(",");
    //            foreach (var role in rolesArray)
    //            {
    //                foreach (var userrole in roles)
    //                {
    //                    if (role == userrole)
    //                    {
    //                        _trace.TraceMe(nameof(OnAuthorizationInvoke), _isTracingRequired, "Role matched!".Information());
    //                        matched = true;
    //                        break;
    //                    }

    //                }
    //                if (matched)
    //                    break;
    //            }

    //            if ((!matched && rolesArray[0] == "") || matched)
    //            {
    //                //filterContext.HttpContext.Response.Headers.Add("authToken", token);
    //                //filterContext.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");
    //                //filterContext.HttpContext.Session.SetString("AuthStatus", "Authorized");
    //                //filterContext.HttpContext.Response.Headers.Add("storeAccessibility", "Authorized");

    //                filterContext.HttpContext.Response.Headers.Add("authToken", token);
    //                filterContext.HttpContext.Response.Headers.Add("AuthStatus", System.Enum.GetName(typeof(AuthStatus), AuthStatus.Authrorized));
    //                filterContext.HttpContext.Response.Headers.Add("storeAccessibility", System.Enum.GetName(typeof(AuthStatus), AuthStatus.Authrorized));
    //                filterContext.HttpContext.Session.SetString("AuthStatus", System.Enum.GetName(typeof(AuthStatus), AuthStatus.Authrorized));

    //                return;
    //            }
    //            filterContext.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
    //            //}
    //        }

    //        // }


    //        filterContext.Result = new RedirectToRouteResult(
    //            new RouteValueDictionary
    //            {
    //            { "area", "Home" },
    //            { "controller", "Home" },
    //            { "action", "AccessDenied" },
    //            { "returnUrl" ,filterContext.HttpContext.Request.Path.Value }
    //                //area=  "Identity",
    //                //controller = "Login",
    //                //action = "Login",
    //                //returnUrl = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped)

    //            });
    //        _trace.TraceMe(nameof(OnAuthorizationInvoke), _isTracingRequired, OperationEnd.ToCss(), "Setting session end!".ToCss());
    //    }
    //    catch (Exception ex)
    //    {
    //        _trace.TraceMe(nameof(OnAuthorizationInvoke), _isTracingRequired, OperationEnd.ToCss(), ex.Message.Information());
    //        filterContext.Result = new RedirectToRouteResult(
    //                                new RouteValueDictionary
    //                                {
    //                                            { "area", "Identity" },
    //                                            { "controller", "Account" },
    //                                            { "action", "Login" },
    //                                            { "returnUrl" ,filterContext.HttpContext.Request.Path.Value }


    //                                });
    //    }
    //    _trace.TraceMe(nameof(OnAuthorizationInvoke), _isTracingRequired, OperationEnd.ToCss(), "Setting session end!".ToCss());
    //}
}


