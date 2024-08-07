using Microsoft.AspNetCore.Http;

namespace SharedLibrary.Services;

public class StateManagement : IStateManagement
{
    IHttpContextAccessor _httpContext;
    public StateManagement(IHttpContextAccessor context)
    {
        _httpContext = context;
    }

    /// <summary>  
    /// set the cookie  
    /// </summary>  
    /// <param name="key">key (unique indentifier)</param>  
    /// <param name="value">value to store in cookie object</param>  
    /// <param name="expireTime">expiration time</param>  
    public void SetCookie(string key, string value, int? expireTime = 0)
    {

        var cookieOptions = new CookieOptions()
        {
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = expireTime != 0 ? DateTime.Now.AddMinutes(expireTime.Value) : DateTime.Now.AddMinutes(40)
        };
        _httpContext?.HttpContext?.Response.Cookies.Append(key, value, cookieOptions);
    }


    /// <summary>
    /// remove cookie
    /// </summary>
    /// <param name="cookieKey">cookie key</param>
    /// <returns>return void</returns>
    public void RemoveCookie(string key)
    {
        _httpContext?.HttpContext?.Response.Cookies.Delete(key);
    }


    /// <summary>
    /// get cookie
    /// </summary>
    /// <param name="cookieKey">cookie key</param>
    /// <returns>return cookie value</returns>
    public string? GetCookie(string cookieKey)
    {
        //read cookie from IHttpContextAccessor  
        return _httpContext?.HttpContext?.Request?.Cookies["key"];
    }
}