using GenericFunction;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeMicroService.Controllers.Base;

public abstract class ApiBaseController : ControllerBase
{
    protected readonly bool IsTracingRequired = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("IsTraceRequired");
}
