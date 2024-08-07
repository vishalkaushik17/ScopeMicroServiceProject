using GenericFunction;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Mvc;
namespace SchoolLibraryMicroService.Controllers.Base;

public abstract class ApiBaseController : ControllerBase
{
    protected readonly ITrace _trace;
    internal readonly bool _isTracingRequired;

    public ApiBaseController(ITrace trace)
    {
        this._trace = trace;
        _isTracingRequired = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("IsTraceRequired");
    }
}
