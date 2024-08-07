// Ignore Spelling: App

using GenericFunction;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Mvc;
using ScopeCoreWebApp.Data;
using ScopeCoreWebApp.Middleware;
using SharedLibrary.Services;
using SharedLibrary.Services.CustomFilters;
using VModelLayer;
using static GenericFunction.CommonMessages;


namespace ScopeCoreWebApp.Areas.Identity.Controller;

[Area("Identity")]

public class AccountController : BaseAppController //Microsoft.AspNetCore.Mvc.Controller
{
	private readonly IConfiguration _configuration;
	private readonly IWebApiExecutor _webApiExecutor;
	private int _roleCount;
	private string _apiEndCallLogs;
	//string _scopeId = string.Empty;
	public AccountController(
		IWebHostEnvironment hostingEnvironment,
		IConfiguration configuration,
		ITrace trace, IHttpContextAccessor httpContextAccessors, ICurrentUserService currentUserService
		, IStateManagement session, IWebApiExecutor webApiExecutor)
		: base(trace, httpContextAccessors, currentUserService, session, hostingEnvironment)
	{
		_configuration = configuration;
		this._webApiExecutor = webApiExecutor;
	}

	public IActionResult Register()
	{
		var lvm = new LoginModel();
		return View("Login", lvm);
	}

	[HttpPost]
	public IActionResult LogOut()
	{
		_currentUserService.RemoveAuthorizeSession(_httpContextAccessor.HttpContext);
		return RedirectToAction("Index", "Home", new { area = "Home" });
	}

	[HttpGet]
	public IActionResult Login()
	{
		return View(new LoginModel());
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	[AllowAnonymous]
	public async Task<IActionResult> Login([FromForm] LoginModel model)
	{
		_scopeId = Guid.NewGuid().ToString("N");
		_sessionId = _httpContextAccessor.HttpContext?.Session?.Id;

		_httpContextAccessor.HttpContext.SetHeader("TokenScopeId", _scopeId);
		_httpContextAccessor.HttpContext.SetHeader("TokenSessionId", _sessionId);
		_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess(), "User interface www");

		_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"{SessionEnvironment.TokenSessionId.MarkProcess()}:{_sessionId}");

		var apiResult = await _webApiExecutor.InvokePost<LoginModel>("login", model, model.Username, _sessionId, _scopeId, _trace);


		//success response logs

		if (apiResult.Status == Status.Success)
		{
			_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "API Response Status Code :".MarkProcess(), apiResult.StatusCode);
			_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "Setting session keys".MarkProcess(), apiResult?.StatusCode + " " + apiResult?.Message);

			//set session for success LoggedIn User
			_currentUserService.SetAuthorizeSession(_httpContextAccessor?.HttpContext, apiResult?.Result);


			_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess(), "Login process executed at UI!");

			_apiEndCallLogs = _trace.GetTraceLogs("");

			string ExecutionLogs = _apiEndCallLogs.Replace("{{replaceApiResponse}}", apiResult.Log);

			_httpContextAccessor.HttpContext.Session.SetString("ExecutionLogs", ExecutionLogs);

			return RedirectToAction("IndexDashboard", "Home", new { area = "Dashboard" });
		}

		_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess(), "Login process executed at UI with errors!");
		_apiEndCallLogs = _trace.GetTraceLogs("");

		if (apiResult.Message.Contains("Using password: YES"))
		{
			apiResult.Message = "Database connectivity issue!";
		}

		model.Message = apiResult?.Message;
		TempData["ExecutionLogs"] = apiResult.Log + _apiEndCallLogs + _trace.GetTraceLogs("");

		return View(model);
	}
}
