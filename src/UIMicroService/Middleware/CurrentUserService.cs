using GenericFunction.Constants.Keys;
using GenericFunction.Enums;
using Microsoft.AspNetCore.Http.Features;
using ModelTemplates.RequestNResponse.Accounts;
using System.Net;

namespace ScopeCoreWebApp.Middleware;

public class CurrentUserService : ICurrentUserService
{
	public void RemoveAuthorizeSession(HttpContext filterContext)
	{
		filterContext.Session.Clear();
		filterContext.Response.Headers.Clear();

		filterContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
		filterContext.Response.HttpContext.Features
			.Get<IHttpResponseFeature>()!.ReasonPhrase = "Not Authorized";
	}

	public void SetAuthorizeSession(HttpContext filterContext, ApplicationUserDtoModel result)
	{

		//new Claim(UserId, user.Id), //user specific id eg USR-XXXX
		//        new Claim(ClientId, user.CompanyId), // eg CMP-XXX
		//        new Claim(UserName, user.UserName), // vishalkaushk@cc.in
		//        new Claim(ClientName, user.Company.Name), // coding company 
		//        new Claim(UserEmail, user.Email), // user personal email
		//        new Claim(ClientEmail, user.Company.Email), //company email id
		//        new Claim(CompanyTypeId, user.CompanyTypeId), //CPT-XXXX
		//        new Claim(TokenSessionId, sessionId), //sesion from which user is going to access the webapi 
		//        new Claim(TokenScopeId, scopeId), // it used for tracing and other activity of user



		//here we are adding response message at the time of successful login

		//filterContext.Session.SetString("Username", result.UserName);
		//filterContext.Session.SetString("FirstName", result.FirstName);
		//filterContext.Session.SetString("LastName", result.LastName);
		//filterContext.Session.SetString("Email", result.UserName);
		//filterContext.Session.SetObjectAsJson("Roles", result.Roles);

		//filterContext.Session.SetString("_Token", result.JwtToken);
		//filterContext.Session.SetString("ClientName", result.ClientName);
		filterContext.Session.SetString(ContextKeys.ClientId, result.ClientId);
		//filterContext.Session.SetString("AuthStatus", "Authorized");
		//filterContext.Session.SetString("Website", result.WebSite);
		//var newSessionId = filterContext.Session.Id;
		//result.TokenSessionId = newSessionId;
		filterContext.Session.SetString(ContextKeys.TokenSessionId, result.TokenSessionId); //adding new session id
		filterContext.Session.SetObjectAsJsonInSession(ContextKeys.AuthSessionObject, result);
		filterContext.Session.SetObjectAsJsonInSession(ContextKeys.AuthStatus, AuthStatus.Authorized);

		filterContext.Session.SetString(ContextKeys.TokenSessionId, result.TokenScopeId);
		filterContext.Session.SetString(ContextKeys.UserId, result.UserId);

		filterContext.Response.StatusCode = (int)HttpStatusCode.OK;
		filterContext.Response.HttpContext.Features
			.Get<IHttpResponseFeature>()!.ReasonPhrase = "Authorized";
	}


	public ApplicationUserDtoModel GetAuthorizeSession(HttpContext filterContext)
	{
		return filterContext?.Session?.GetObjectFromJsonFromSession<ApplicationUserDtoModel>(ContextKeys.AuthSessionObject);
	}


}