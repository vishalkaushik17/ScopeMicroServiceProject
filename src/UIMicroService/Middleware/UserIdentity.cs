//using GenericFunction.ResultObject;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.EntityFrameworkCore;
//using ModelTemplates.RequestNResponse.Accounts;

//namespace ScopeCoreWebApp.Middleware;

///// <summary>
///// This class is responsible to get all the information related to logged in user.
///// </summary>
//public static class UserIdentity
//{
//    public static bool IsSignedIn(this HttpContext context)
//    {
//        var isAuth = context?.Session.GetString("AuthStatus")?? String.Empty;
//        if (isAuth == "Authorized")
//        {
//            return true;
//        }

//        return false;
//    }

//    public static ApplicationUserDtoModel? GetLoggedInUserInfoFromContext(this HttpContext context)
//    {
//        if (context == null)
//        {
//            return default;
//        }
//        return context.Session.GetObjectFromJson<ApplicationUserDtoModel>("AuthSessionObject");
//    }

//    /// <summary>
//    /// Get user full name eg. vishal kaushik
//    /// </summary>
//    /// <param name="context"></param>
//    /// <returns></returns>
//    public static string GetFullName(this HttpContext context)
//    {
//        var firstName = context.GetLoggedInUserInfoFromContext()?.FirstName;
//        var lastName = context.GetLoggedInUserInfoFromContext()?.LastName;
//        return $"{firstName} {lastName}" ?? string.Empty;
//    }

//    /// <summary>
//    /// Get User name eg: admin@cc.in
//    /// </summary>
//    /// <param name="context"></param>
//    /// <returns></returns>
//    public static string GetUserName(this HttpContext context)
//    {
//        return context.GetLoggedInUserInfoFromContext()?.UserName ?? "Default";
//    }


//    /// <summary>
//    /// Get User email eg: user@company.in
//    /// </summary>
//    /// <param name="context"></param>
//    /// <returns></returns>
//    public static string GetUserEmail(this HttpContext context)
//    {
//        return context.GetLoggedInUserInfoFromContext()?.UserEmail?? string.Empty;
//    }

//    /// <summary>
//    /// Get User personal email eg: user@gmail.in
//    /// </summary>
//    /// <param name="context"></param>
//    /// <returns></returns>
//    public static string GetUserPersonalEmail(this HttpContext context)
//    {
//        return context.GetLoggedInUserInfoFromContext()?.PersonalEmailId?? string.Empty;
//    }



//    /// <summary>
//    /// Get logged in user id eg. USR-XX-XX-XXXXX
//    /// </summary>
//    /// <param name="context"></param>
//    /// <returns></returns>
//    public static string GetUserId(this HttpContext context)
//    {
//        return context.GetLoggedInUserInfoFromContext()?.UserId ?? "Default";
//    }

//    /// <summary>
//    /// Get current session id eg: f2134dfsx34dfrfddf
//    /// </summary>
//    /// <param name="context"></param>
//    /// <returns></returns>
//    public static string GetSessionId(this HttpContext context)
//    {
//        return context?.Session?.Id ?? new Guid().ToString();
        
//    }        

//    /// <summary>
//    /// Get Token from the httpContext, which sent by api after successful authentication.
//    /// </summary>
//    /// <param name="context"></param>
//    /// <returns></returns>
//    public static string GetToken(this HttpContext context)
//    {
//        return context.GetLoggedInUserInfoFromContext()?.JwtToken ?? string.Empty;
//    }

//    /// <summary>
//    /// Get Client Name eg. Coding Company, Xyz School
//    /// </summary>
//    /// <param name="context"></param>
//    /// <returns></returns>
//    public static string GetClientName(this HttpContext context)
//    {
//        return context.GetLoggedInUserInfoFromContext()?.ClientName ?? string.Empty;
//    }
    
    
//    /// <summary>
//    /// Get client id eg. CMP-XX-XX-XX123424
//    /// </summary>
//    /// <param name="context"></param>
//    /// <returns></returns>
//    public static string GetClientId(this HttpContext context)
//    {
//        return context.GetLoggedInUserInfoFromContext()?.ClientId?? string.Empty;
//    }

//    /// <summary>
//    /// Get website of the client.
//    /// </summary>
//    /// <param name="context"></param>
//    /// <returns></returns>
//    public static string GetWebsite(this HttpContext context)
//    {
//        return context.GetLoggedInUserInfoFromContext()?.WebSite ?? string.Empty;
//    }


//    public static ApplicationUserDtoModel? User(this HttpContext context)
//    {
//        var User = context?.Session.GetObjectFromJson<ApplicationUserDtoModel>("AuthSessionObject");
//        if (User != null)
//        {
//            return User;
//        }

//        return null;
//    }
//    public static bool UserInRole(this HttpContext? context, params string[] roles)
//    {

//        if (context!=null && context.IsSignedIn()) return false;

//        var userRoles = context?.Session?.GetObjectFromJson<List<string>>("Roles")?.ToList();


//        if (userRoles?.Count() == 0 || userRoles == null)
//            return false;

//        bool found = false;
//        foreach (var r in userRoles)
//        {
//            foreach (var role in roles)
//            {
//                if (role == r)
//                {
//                    found = true;
//                    break;
//                }
//            }
//            if (found)
//                break;
//        }
//        if (found) return true;

//        return false;
//    }
//}