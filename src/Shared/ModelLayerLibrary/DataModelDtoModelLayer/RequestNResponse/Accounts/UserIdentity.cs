using GenericFunction;
using GenericFunction.Constants.AppConfig;
using GenericFunction.Constants.Keys;
using GenericFunction.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
namespace ModelTemplates.RequestNResponse.Accounts;

//need to work on item as string in this class.

/// <summary>
/// This class is responsible to get all the information related to logged in user.
/// </summary>
public static class UserIdentity
{
	public static void SetContextItemAsJson(this HttpContext context, string key, object value)
	{
		if (context.Items.ContainsKey(key))
		{
			context.Items.Remove(key);
		}
		context.Items.Add(key, JsonConvert.SerializeObject(value, Formatting.Indented,
	new JsonSerializerSettings()
	{
		ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
	}));
	}

	public static T GetContextItemAsJson<T>(this HttpContext context, string key)
	{
		var value = context.Items?[key] as string;

		return value == null ? default(T) : value.FromJsonToObject<T>();
	}

	public static bool IsSignedIn(this HttpContext? context)
	{

		var isAuth = context.Session.GetObjectFromJsonFromSession<AuthStatus>(ContextKeys.AuthStatus);
		if (isAuth == AuthStatus.Authorized)
		{
			return true;
		}
		return false;
	}

	public static ApplicationUserDtoModel? GetLoggedInUserInfoFromContext(this HttpContext context)
	{
		try
		{

			if (context == null)
			{
				return default;
			}
			return context.Session?.GetObjectFromJsonFromSession<ApplicationUserDtoModel>(ContextKeys.AuthSessionObject);
		}
		catch (Exception ex)
		{

			return default;
		}

	}

	/// <summary>
	/// Get user full name eg. vishal kaushik
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public static string GetFullName(this HttpContext context)
	{
		var firstName = context.GetLoggedInUserInfoFromContext()?.FirstName;
		var lastName = context.GetLoggedInUserInfoFromContext()?.LastName;
		return $"{firstName} {lastName}" ?? string.Empty;
	}

	/// <summary>
	/// Get User name eg: admin@cc.in
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public static string GetUserName(this HttpContext context)
	{
		return context.GetLoggedInUserInfoFromContext()?.UserName ?? "Default";
	}


	/// <summary>
	/// Get User email eg: user@company.in
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public static string GetUserEmail(this HttpContext context)
	{
		return context.GetLoggedInUserInfoFromContext()?.UserEmail ?? string.Empty;
	}

	/// <summary>
	/// Get User personal email eg: user@gmail.in
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public static string GetUserPersonalEmail(this HttpContext context)
	{
		return context.GetLoggedInUserInfoFromContext()?.PersonalEmailId ?? string.Empty;
	}



	/// <summary>
	/// Get logged in user id eg. USR-XX-XX-XXXXX
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public static string GetUserId(this HttpContext? context)
	{
		if (context == null)
			return string.Empty;

		string value = context.GetHeader(ContextKeys.UserId);
		if (string.IsNullOrWhiteSpace(value))
		{
			value = context.GetLoggedInUserInfoFromContext()?.UserId ?? "Default";
		}
		if (string.IsNullOrWhiteSpace(value))
		{
			value = context?.GetHeader(ContextKeys.UserId) ?? string.Empty;
		}
		return string.IsNullOrWhiteSpace(value) ? "Default" : value;

	}
	//sessionId = context.GetRequestHeaders("TokenSessionId");
	//    scopeId = context.GetRequestHeaders("TokenScopeId");

	/// <summary>
	/// Get current session id eg: f2134dfsx34dfrfddf
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public static string? GetSessionId(this HttpContext? context)
	{
		if (context == null)
			return string.Empty;

		string value = context.GetHeader(ContextKeys.TokenSessionId);
		if (string.IsNullOrWhiteSpace(value))
		{
			value = context.GetLoggedInUserInfoFromContext()?.TokenSessionId ?? "Default";
		}
		if (string.IsNullOrWhiteSpace(value))
		{
			value = context?.Items[ContextKeys.TokenSessionId] as string ?? string.Empty;
		}
		return value;
	}

	/// <summary>
	/// Using this id we are able to identify and retrieve logs for user who is visiting our application.
	/// </summary>
	/// <param name="context">HttpContext</param>
	/// <returns>string</returns>
	public static string? GetScopeId(this HttpContext context)
	{
		if (context == null)
			return string.Empty;

		string value = context.GetHeader(ContextKeys.TokenScopeId);
		if (string.IsNullOrWhiteSpace(value))
		{
			value = context.GetLoggedInUserInfoFromContext()?.TokenScopeId ?? "Default";
		}
		if (string.IsNullOrWhiteSpace(value))
		{
			value = context?.Items[ContextKeys.TokenScopeId] as string ?? string.Empty;
		}
		return value;
	}

	/// <summary>
	/// Get Token from the httpContext, which sent by api after successful authentication.
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public static string GetToken(this HttpContext context)
	{
		string value = context.GetHeader(ContextKeys.Authorization);
		if (string.IsNullOrWhiteSpace(value))
		{
			value = context.GetLoggedInUserInfoFromContext()?.JwtToken ?? string.Empty;
		}
		if (string.IsNullOrWhiteSpace(value))
		{
			value = context?.Items[ContextKeys.Authorization] as string ?? string.Empty;
		}
		return value;
		//return context.GetLoggedInUserInfoFromContext()?.JwtToken ?? string.Empty;
	}

	/// <summary>
	/// Get Client Name eg. Coding Company, Xyz School
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public static string GetClientName(this HttpContext context)
	{
		string value = context.GetHeader(ContextKeys.ClientName);
		if (string.IsNullOrWhiteSpace(value))
		{
			value = context.GetLoggedInUserInfoFromContext()?.ClientName ?? string.Empty;
		}
		if (string.IsNullOrWhiteSpace(value))
		{
			value = context?.Items[ContextKeys.ClientName] as string ?? string.Empty;
		}

		return string.IsNullOrWhiteSpace(value) ? "Default" : value;
		//return context.GetLoggedInUserInfoFromContext()?.ClientName ?? string.Empty;
	}

	/// <summary>
	/// Get website of the client.
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public static string GetWebsite(this HttpContext context)
	{
		return context.GetLoggedInUserInfoFromContext()?.WebSite ?? string.Empty;
	}
	/// <summary>
	/// Get Client Name eg. Coding Company, Xyz School
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public static string GetClientId(this HttpContext? context)
	{
		string value = context.GetHeader(ContextKeys.ClientId);
		if (string.IsNullOrWhiteSpace(value))
		{
			value = context.GetLoggedInUserInfoFromContext()?.ClientId ?? "Common";
		}
		if (string.IsNullOrWhiteSpace(value))
		{
			value = context?.Items[ContextKeys.ClientId] as string ?? "Common";
		}

		return string.IsNullOrWhiteSpace(value) ? "Common" : value;
		//return context.GetLoggedInUserInfoFromContext()?.ClientId ?? "Default";
	}

	/// <summary>
	/// Get Software running setup environment.
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public static string GetSetupEnvironment(this HttpContext context)
	{
		return context.GetHeader(SoftwareEnvironment.ASPNETCORE_ENVIRONMENT) ?? string.Empty;
	}

	/// <summary>
	/// Set Software running setup environment.
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public static void SetSetupEnvironment(this HttpContext context, string setupEnvironment)
	{
		context.SetHeader(SoftwareEnvironment.ASPNETCORE_ENVIRONMENT, setupEnvironment);
	}

	/// <summary>
	/// Check logged in user is in role
	/// </summary>
	/// <param name="context"></param>
	/// <param name="roles"></param>
	/// <returns></returns>
	public static bool UserInRole(this HttpContext context, params string[] roles)
	{

		if (context != null && context.IsSignedIn()) return false;

		var userRoles = context.GetUserRoles();


		if (userRoles?.Count() == 0 || userRoles == null)
			return false;

		bool found = false;
		foreach (var r in userRoles)
		{
			foreach (var role in roles)
			{
				if (role == r)
				{
					found = true;
					break;
				}
			}
			if (found)
				break;
		}
		if (found) return true;

		return false;
	}

	/// <summary>
	/// get list of roles for logged in user
	/// </summary>
	/// <param name="context"></param>
	/// <returns></returns>
	public static List<string?> GetUserRoles(this HttpContext? context)
	{

		if (context != null && context.IsSignedIn())

			return context.GetLoggedInUserInfoFromContext()?.Roles;
		return new List<string?>();
	}

	/// <summary>
	/// check user is in assigned role on ui module
	/// </summary>
	/// <param name="context"></param>
	/// <param name="roles"></param>
	/// <returns></returns>
	public static bool CheckUserInRole(this HttpContext? context, string[] roles)
	{
		var userRoles = context.GetUserRoles();
		foreach (var item in roles)
		{
			if (userRoles.Any(m => m.Contains(item)))
				return true;

		}
		return false;
	}

}