using GenericFunction;
using Microsoft.AspNetCore.Mvc;
namespace CommonMicroService.Controllers.Base;

/// <summary>
/// Base api controller
/// </summary>
public abstract class ApiBaseController : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    protected ApiBaseController()
    {
        AppSettingsConfigurationManager.AppSetting.GetValue<bool>("IsTraceRequired");
    }

}
