////using Microsoft.AspNetCore.Mvc.ApplicationModels;

////public class ModelStateValidatorConvension : IApplicationModelConvention
////{
////    public void Apply(ApplicationModel application)
////    {
////        foreach (var controllerModel in application.Controllers)
////        {
////            controllerModel.Filters.Add(new ModelStateValidationFilterAttribute());
////        }
////    }
////}

//using GenericFunction;
//using GenericFunction.ResultObject;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Controllers;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.Extensions.Configuration;
//using System.Reflection;
//using UnitOfWork.DI;
//using static GenericFunction.CommonMessages;

//namespace UnitOfWork.RequiredAttributes;

//public class ValidationFilterAttribute : IActionFilter
//{
//    private readonly ITrace _trace;
//    private readonly IConfiguration _configuration;
//    internal readonly bool _IsTracingRequired;
//    public ValidationFilterAttribute(ITrace trace, IConfiguration config)
//    {
//        _trace = trace;
//        _configuration = config;
//        _IsTracingRequired = AppSettingsConfigurationManager.AppSetting.GetValue<bool>("IsTraceRequired");
//    }


//    public void OnActionExecuting(ActionExecutingContext context)
//    {
//        if (!context.ModelState.IsValid)
//        {
//            _trace.TraceMe(this.ControllerContext.RouteData, _IsTracingRequired, OperationStart.ToCss());

//            _trace.TraceMe(ControllerContext.RouteData, _IsTracingRequired
//                , GeneratingModelStateMessages.ToCss());


//            context.Result = new UnprocessableEntityObjectResult(context.ModelState);
//            int count = 1;
//            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
//            // context.Controller as Controller
//            var actionName = descriptor?.ActionName;
//            var controllerName = descriptor?.ControllerName;

//            foreach (var modelValues in context.ModelState.Values)
//            {

//                foreach (var err in modelValues.Errors)
//                {
//                    _trace.TraceMe(, _IsTracingRequired
//                        , count + " : " + err.ErrorMessage.ToCss() + err.Exception);
//                    count++;

//                }
//            }
//            _trace.TraceMe(new CurrentMethod
//            {
//                Controller = this.GetType().Name,
//                Method = MethodBase.GetCurrentMethod()?.Name
//            }, _IsTracingRequired
//                , ("Total  " + context.ModelState.Values.Count()).ToCss() + GeneratedModelStateMessages.ToCss());

//            context.Result = new UnprocessableEntityObjectResult(new ResponseDto<IActionResult>
//            {
//                Result = context.Result,
//                RecordCount = context.ModelState.Count,
//                Status = Status.Failed,
//                Log = _trace.GetTraceLogs(),
//                TimeConsumption = _trace.TimeConsumption(),
//                Message = InvalidModelState,
//                StatusCode = StatusCodes.Status422UnprocessableEntity

//            });


//            //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

//        }

//        var accessToken = context.HttpContext.GetTokenAsync("access_token").Result;


//        ITokenService tokenService = new TokenService();
//        if (!tokenService.ValidateToken(_configuration[IssuerSigningKey].ToString(), _configuration[ValidIssuer].ToString(), _configuration[ValidAudience], accessToken, out string message, out Exception ex))
//        {
//            context.Result = new UnauthorizedObjectResult(new ResponseDto<IActionResult>
//            {
//                Result = context.Result,
//                RecordCount = 0,
//                Status = Status.Failed,
//                Log = _trace.GetTraceLogs(),
//                TimeConsumption = _trace.TimeConsumption(),
//                Message = message,
//                Exception = ex,
//                MessageType = MessageType.Exception,
//                StatusCode = StatusCodes.Status401Unauthorized

//            });
//        }

//    }

//    public void OnActionExecuted(ActionExecutedContext context)
//    {


//    }
//}