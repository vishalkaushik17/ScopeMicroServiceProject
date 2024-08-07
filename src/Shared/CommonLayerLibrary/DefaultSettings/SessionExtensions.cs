//using System.Text;
//using GenericFunction.Enums;
//using Microsoft.AspNetCore.Http;
//using Newtonsoft.Json;

//namespace GenericFunction.DefaultSettings;


//public static class SessionExtensions
//{
//    public static void SetObjectAsJson(this ISession session, string key, object value)
//    {
//        session.SetString(key, JsonConvert.SerializeObject(value));
//    }

//    public static T GetObjectFromJson<T>(this ISession session, string key)
//    {
//        var value = session.GetString(key);

//        return value == null ? default(T) : value.FromJsonToObject<T>();
//    }
//}
////public class SessionManagement
////{
////    private readonly IHttpContextAccessor _httpContextAccessor;

////    public SessionManagement(IHttpContextAccessor httpContextAccessor)
////    {
////        this._httpContextAccessor = httpContextAccessor;
////    }
////    public static IHttpContextAccessor GetCurrentContext() {
////        return _httpContextAccessor;
////    }
////}
///// <summary>
///// This class is responsible to get all the information related to logged in user.
///// </summary>
//public static class UserIdentity
//{
//    public static bool IsSignedIn(this HttpContext context)
//    {
//        var isAuth = context.Session?.GetObjectFromJson<AuthStatus>("AuthStatus");
//        if (isAuth == AuthStatus.Authorized)
//        {
//            return true;
//        }
//        return false;
//    }

//    public static ApplicationUserDtoModel? GetLoggedInUserInfoFromContext(this HttpContext context)
//    {
//        try
//        {

//            if (context == null)
//            {
//                return default;
//            }
//            return context.Session?.GetObjectFromJson<ApplicationUserDtoModel>("AuthSessionObject");
//        }
//        catch (Exception ex)
//        {

//            return default;
//        }

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
//        return context.GetLoggedInUserInfoFromContext()?.UserEmail ?? string.Empty;
//    }

//    /// <summary>
//    /// Get User personal email eg: user@gmail.in
//    /// </summary>
//    /// <param name="context"></param>
//    /// <returns></returns>
//    public static string GetUserPersonalEmail(this HttpContext context)
//    {
//        return context.GetLoggedInUserInfoFromContext()?.PersonalEmailId ?? string.Empty;
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
//        //return context?.Items["SessionId"] as string ?? string.Empty;
//        return context.GetLoggedInUserInfoFromContext()?.TokenSessionId ?? "Default";

//    }

//    /// <summary>
//    /// Get current scope id eg: f2134dfsx34dfrfddf
//    /// </summary>
//    /// <param name="context"></param>
//    /// <returns></returns>
//    public static string GetScopeId(this HttpContext context)
//    {
//        return context.GetLoggedInUserInfoFromContext()?.TokenScopeId ?? "Default";

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
//    /// Get Client Name eg. Coding Company, Xyz School
//    /// </summary>
//    /// <param name="context"></param>
//    /// <returns></returns>
//    public static string GetClientId(this HttpContext context)
//    {
//        return context.GetLoggedInUserInfoFromContext()?.ClientId ?? "Default";
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


//    /// <summary>
//    /// Check logged in user is in role
//    /// </summary>
//    /// <param name="context"></param>
//    /// <param name="roles"></param>
//    /// <returns></returns>
//    public static bool UserInRole(this HttpContext context, params string[] roles)
//    {

//        if (context != null && context.IsSignedIn()) return false;

//        var userRoles = context.GetUserRoles();


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

//    /// <summary>
//    /// get list of roles for logged in user
//    /// </summary>
//    /// <param name="context"></param>
//    /// <returns></returns>
//    public static List<string> GetUserRoles(this HttpContext context)
//    {

//        if (context != null && context.IsSignedIn()) return new List<string>();

//        return context.GetLoggedInUserInfoFromContext()?.Roles;

//    }
//}