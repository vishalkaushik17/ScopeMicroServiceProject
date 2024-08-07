//using GenericFunction.ResultObject;
//using Microsoft.AspNetCore.Http.Features;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using ModelTemplates.RequestNResponse.Accounts;
//using System.Net;

//namespace ScopeCoreWebApp.CustomAttributes
//{
//    public class CustomAuthorization : Attribute, IAuthorizationFilter
//    {
//        //need to check

//        /// <summary>  
//        /// This will Authorize Username  
//        /// </summary>  
//        /// <returns></returns>  
//        public void OnAuthorization(AuthorizationFilterContext filterContext)
//        {
//            if (filterContext.HttpContext.Request.Path != null)
//            {
//                var username = filterContext.HttpContext.Session.GetObjectFromJson<ApplicationUserDtoModel>("AuthSessionObject")?.UserName;
//                var token = filterContext.HttpContext.Session.GetString("_Token");



//                if (token != null && !string.IsNullOrWhiteSpace(username))
//                {
//                    var roles = filterContext.HttpContext.Session.GetObjectFromJson<ApplicationUserDtoModel>("AuthSessionObject").Roles;
//                    if (IsValidToken(token))
//                    {
//                        filterContext.HttpContext.Response.Headers.Add("authToken", token);
//                        filterContext.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");

//                        filterContext.HttpContext.Response.Headers.Add("storeAccessiblity", "Authorized");
//                        //filterContext.Result = new RedirectToRouteResult(
//                        //    new RouteValueDictionary
//                        //    {
//                        //        { "area", "Home" },
//                        //        { "controller", "Home" },
//                        //        { "action", "Index" },

//                        //    });
//                        return;
//                    }
//                    else
//                    {
//                        filterContext.HttpContext.Response.Headers.Add("authToken", token);
//                        filterContext.HttpContext.Response.Headers.Add("AuthStatus", "NotAuthorized");

//                        filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
//                        filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>()!.ReasonPhrase = "Not Authorized";
//                        filterContext.Result = new UnauthorizedResult();
//                        filterContext.Result = new RedirectToRouteResult(
//                            new RouteValueDictionary
//                            {
//                                { "area", "Identity" },
//                                { "controller", "Account" },
//                                { "action", "Login" }
//                            });

//                        return;
//                    }

//                }

//            }


//            filterContext.Result = new RedirectToRouteResult(
//                       new RouteValueDictionary
//                       {
//                                { "area", "Identity" },
//                                { "controller", "Account" },
//                                { "action", "Login" }
//                       });
//            return;

//        }


//        public bool IsValidToken(string authToken)
//        {
//            //validate _Token here  
//            return true;
//        }
//    }
//}
