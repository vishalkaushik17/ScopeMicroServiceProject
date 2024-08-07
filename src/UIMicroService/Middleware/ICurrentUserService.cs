using ModelTemplates.RequestNResponse.Accounts;

namespace ScopeCoreWebApp.Middleware;

public interface ICurrentUserService
{


    void SetAuthorizeSession(HttpContext filterContext, ApplicationUserDtoModel result);
    void RemoveAuthorizeSession(HttpContext filterContext);
    ApplicationUserDtoModel GetAuthorizeSession(HttpContext filterContext);


}