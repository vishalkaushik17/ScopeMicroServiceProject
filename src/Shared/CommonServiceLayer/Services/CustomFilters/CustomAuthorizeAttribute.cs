using GenericFunction;
using GenericFunction.Constants.Keys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ModelTemplates.EntityModels.Application;
using ModelTemplates.RequestNResponse.Accounts;

namespace SharedLibrary.Services.CustomFilters;

//public class AuthorizeRolesAttribute : Attribute, IAuthorizationFilter
//{
//    private readonly string? _roles;
//    public AuthorizeRolesAttribute(params string[] roles)
//    {
//        _roles = String.Join(",", roles);
//    }
//    public void OnAuthorization(AuthorizationFilterContext context)
//    {
//        // skip authorization if action is decorated with [AllowAnonymous] attribute
//        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
//        if (allowAnonymous)
//            return;

//        // authorization
//        var account = (ApplicationUser)context.HttpContext.Items["Account"];
//        JtwTokenContainerResponse ClientInfo = (JtwTokenContainerResponse)context.HttpContext.Items["ClientInfo"];
//        if (account == null || (ClientInfo.Roles.Any() && !ClientInfo.Roles.Contains(_roles)))
//        {
//            // not logged in or role not authorized
//            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
//        }
//    }
//}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{


    private readonly IList<string> _roles;

    public CustomAuthorizeAttribute(params string[]? roles)
    {
        _roles = roles ?? new string[] { };
    }


    /// <summary>
    /// this class is used for API service only.
    /// </summary>
    /// <param name="context"></param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {



        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;

        // authorization

        
        bool isDemoExpired = context.HttpContext.GetContextItemAsJson<bool>("IsDemoExpired");
        
        if (isDemoExpired && context.HttpContext.Request.Method == "POST" || context.HttpContext.Request.Method == "PUT" || context.HttpContext.Request.Method == "DELETE")
        {
            context.Result = new JsonResult(new { message = "Demo/Subscription is expired! Data is readonly mode! <strong>Unauthorized!</strong>" }) { StatusCode = StatusCodes.Status401Unauthorized };
            return;
        }
        JtwTokenContainerResponse clientInfo = context.HttpContext.GetContextItemAsJson<JtwTokenContainerResponse>(ContextKeys.ClientInfo)!;
        if (clientInfo == null)
        {
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            return;
        }

        //this is decoded properties from token which is stored by jwtMiddleware
        //JtwTokenContainerResponse? clientInfo = (JtwTokenContainerResponse)context.HttpContext.GetContextItemAsJson<JtwTokenContainerResponse>("ClientInfo")!;
        //if (clientInfo == null)
        //{
        //    context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };

        //    return;
        //}

        bool authorized = false;

        foreach (var role in _roles)
        {
            if (clientInfo.Roles.Contains(role?.ToString()))
            {
                authorized = true;
                break;
            }
        }

        if (_roles.Count == 0) authorized = true;
        if (clientInfo == null || !authorized)
        {
            // not logged in or role not authorized
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}

//[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
//{
//    private readonly IList<Roles> _roles;

//    public CustomAuthorizeAttribute(params Roles[] roles)
//    {
//        _roles = roles ?? new Roles[] { };
//    }

//    public void OnAuthorization(AuthorizationFilterContext context)
//    {
//        // skip authorization if action is decorated with [AllowAnonymous] attribute
//        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
//        if (allowAnonymous)
//            return;

//        // authorization
//        var account = (ApplicationUser)context.HttpContext.Items["Account"];
//        if (account == null || (_roles.Any() && !_roles.Contains(account.Role)))
//        {
//            // not logged in or role not authorized
//            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
//        }
//    }
//}