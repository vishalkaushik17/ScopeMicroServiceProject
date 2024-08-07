namespace SharedLibrary.Services;

public interface IStateManagement
{
    /// <summary>  
    /// set the cookie  
    /// </summary>  
    /// <param name="key">key (unique indentifier)</param>  
    /// <param name="value">value to store in cookie object</param>  
    /// <param name="expireTime">expiration time</param>  
    void SetCookie(string key, string value, int? expireTime);

    /// <summary>
    /// get cookie
    /// </summary>
    /// <param name="cookieKey">cookie key</param>
    /// <returns>return cookie value</returns>
    string? GetCookie(string cookieKey);

    /// <summary>
    /// remove cookie
    /// </summary>
    /// <param name="cookieKey">cookie key</param>
    /// <returns>return void</returns>
    void RemoveCookie(string key);
}