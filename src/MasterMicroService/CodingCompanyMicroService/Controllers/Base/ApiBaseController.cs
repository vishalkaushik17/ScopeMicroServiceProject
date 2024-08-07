using AutoMapper;
using GenericFunction;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelTemplates.EntityModels.Application;
using UnitOfWork.DI;
using static GenericFunction.CommonMessages;
namespace CodingCompanyService.Controllers.Base
{

	/// <summary>
	/// Base controller for all the Api controllers, do not change 
	/// </summary>
	public abstract class ApiBaseController : ControllerBase
	{
		protected internal readonly IMapper _mapper;
		protected internal readonly ITrace _trace;
		protected internal readonly IHttpContextAccessor _HttpContextAccessor;
		protected readonly ITokenService _tService;
		protected internal readonly string? _userId;
		protected internal readonly string? _clientId;
		protected internal readonly string? _sessionId;
		protected internal readonly bool _isTracingRequired;
		protected internal UserManager<ApplicationUser> _UserManager;
		protected ApiBaseController(ITrace trace)
		{
			_trace = trace;
			_isTracingRequired = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("IsTraceRequired");
			_trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess());

		}

	}
}
