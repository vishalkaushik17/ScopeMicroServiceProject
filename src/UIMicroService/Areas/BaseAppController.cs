using GenericFunction;
using GenericFunction.DefaultSettings;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Mvc;
using ModelTemplates.RequestNResponse;
using ModelTemplates.RequestNResponse.Accounts;
using ScopeCoreWebApp.Middleware;
using SharedLibrary.Services;
using static GenericFunction.CommonMessages;
using static GenericFunction.ExtensionMethods;

namespace ScopeCoreWebApp.Areas
{
	public abstract class BaseAppController : Controller
	{
		protected readonly ITrace _trace;
		protected readonly IHttpContextAccessor _httpContextAccessor;
		protected readonly ICurrentUserService _currentUserService;
		protected readonly bool _isTracingRequired;
		protected readonly IStateManagement _cookies;
		protected readonly SPAResponse<string> _sPAResponse;
		protected string _sessionId;
		protected string _scopeId;
		protected string _clientId;
		protected string _userId;
		private ApplicationSettings _applicationSettings;
		protected BaseAppController(ITrace trace,
			IHttpContextAccessor httpContextAccessors, ICurrentUserService currentUserService,
			IStateManagement cookie, IWebHostEnvironment hostingEnvironment)
		{

			_applicationSettings = SettingsConfigHelper.GetOptions<ApplicationSettings>();
			_cookies = cookie;
			_trace = trace;
			_httpContextAccessor = httpContextAccessors;
			//_userId = httpContextAccessors.HttpContext?.User.FindFirst("Id")?.Value;
			_currentUserService = currentUserService;
			_isTracingRequired = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("IsTraceRequired");
			UserIdentity.SetSetupEnvironment(_httpContextAccessor.HttpContext, AppSettingsConfigurationManager.AppSetting.GetValue<string>("ASPNETCORE_ENVIRONMENT"));

			_sPAResponse = new SPAResponse<string>(hostingEnvironment);
			_sPAResponse.Status = true;
			_sPAResponse.ResponseType = GenericFunction.Enums.SPAResponseType.HTML;
			_userId = UserIdentity.GetUserId(_httpContextAccessor.HttpContext);
			_clientId = UserIdentity.GetClientId(_httpContextAccessor.HttpContext);
			_sessionId = UserIdentity.GetSessionId(_httpContextAccessor.HttpContext);
			_scopeId = UserIdentity.GetScopeId(_httpContextAccessor.HttpContext);


		}
		internal protected void AddLogPhase1(bool IsJsModuleAvailable = false, bool IsCssModuleAvailable = false)
		{
			_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess());
			_sPAResponse.FullViewName = $"{this.GetSupportedFileName(this.ControllerContext)}".ToLower();
			_sPAResponse.SupportedFileName = this.GetSupportedFileName(this.ControllerContext);
			_sPAResponse.ViewName = $"{this.GetActionMethod(this.ControllerContext)}".ToLower();

			_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(),
			_isTracingRequired, $"Reading {this.GetFullViewPath(this.ControllerContext)} View.".GetCurrentLineNo());

			_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(),
			_isTracingRequired, $"Reading completed! {this.GetFullViewPath(this.ControllerContext)} View.".GetCurrentLineNo());
			_sPAResponse.IsJsModuleAvailable = IsJsModuleAvailable;//load js module
			_sPAResponse.IsCssModuleAvailable = IsCssModuleAvailable;//load css module

			_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(),
			_isTracingRequired, $"Creating version for js and css files required for : {this.GetFullViewPath(this.ControllerContext)} View.".GetCurrentLineNo());
			_sPAResponse.InitFileVersion();
			_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"JS & CSS Version added!".MarkProcess());
			if (string.IsNullOrWhiteSpace(_sPAResponse.HtmlResponse))
			{
				_sPAResponse.MessageType = SPAMessageType.WAR;
				_sPAResponse.Status = false;
				_sPAResponse.ShowMessage = true;
				_sPAResponse.Message = "View not available!!!";
				_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"{_sPAResponse.Message}: {this.GetFullViewPath(this.ControllerContext)}".MarkProcess(), _sPAResponse?.Status + " " + _sPAResponse?.Message);
			}
		}
		internal protected void CatchLogPhase(Exception ex)
		{
			_sPAResponse.ShowMessage = true;
			_sPAResponse.MessageType = SPAMessageType.ERR;
			_sPAResponse.HtmlResponse = "";
			_sPAResponse.Status = false;
			_sPAResponse.Message = ex.Message;
			_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"{this.GetFullViewPath(this.ControllerContext)} - Exception occurred".MarkProcess(), _sPAResponse?.Status + " " + _sPAResponse?.Message);
		}

		internal protected SPAResponse<string> FinalLogPhase()
		{
			_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess());
			_sPAResponse.Logs = _trace.GetTraceLogs("");
			return _sPAResponse;
		}


		protected string GetSupportedFileName(ControllerContext controllerContext)
		{
			return $"{controllerContext.ActionDescriptor.ControllerName}{controllerContext.ActionDescriptor.ActionName}".ToLower();
		}
		protected string? GetFullViewPath(ControllerContext controllerContext)
		{
			return $"Area: {GetAreaName(controllerContext)} - Controller: {GetControllerName(controllerContext)} - View: {GetActionMethod(controllerContext)}";
		}
		protected string? GetAreaName(ControllerContext controllerContext)
		{
			return controllerContext.RouteData.Values.FirstOrDefault(x => x.Key == "area").Value?.ToString();//.Select(x => x.Value.ToString());
		}
		protected string? GetControllerName(ControllerContext controllerContext)
		{
			return controllerContext.ActionDescriptor.ControllerName;
		}
		protected string? GetActionMethod(ControllerContext controllerContext)
		{
			return controllerContext.ActionDescriptor.ActionName;
		}

	}
}
