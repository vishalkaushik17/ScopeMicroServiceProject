namespace SharedLibrary.Services.CustomFilters;

[AttributeUsage(AttributeTargets.Method)]
public class AllowAnonymousAttribute : Attribute
{ }

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