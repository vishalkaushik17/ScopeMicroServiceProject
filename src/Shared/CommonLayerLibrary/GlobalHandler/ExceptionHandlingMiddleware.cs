using GenericFunction.Constants.Keys;
using GenericFunction.Enums;
using GenericFunction.GlobalHandler.GlobalHandler;
using GenericFunction.GlobalService.EmailService.Contracts;
using GenericFunction.ResultObject;
using GenericFunction.ServiceObjects.EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace GenericFunction.GlobalHandler;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEmailService _emailService;
    private readonly ITrace _trace;
    private readonly IConfiguration _configuration;
    public string? _sessionId { get; set; }


    public ExceptionHandlingMiddleware(IHttpContextAccessor httpContextAccessor, IEmailService emailService, ITrace trace, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _emailService = emailService;
        _trace = trace;
        _configuration = configuration;

    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            e.SendExceptionMailAsync().GetAwaiter().GetResult();
            context.Response.ContentType = "application/json";
            await HandleExceptionAsync(context, e, _httpContextAccessor, _emailService, _trace, _configuration);

        }
    }

    /// <summary>
    /// Exception handling for mvc and api calls
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="_emailService"></param>
    /// <param name="trace"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static async Task HandleExceptionAsync(HttpContext context,
      Exception exception, IHttpContextAccessor httpContextAccessor,
      IEmailService _emailService, ITrace trace, IConfiguration configuration)
    {
        string message;
        Uri? myUri = null;
        var exceptionType = exception.GetType();
        var request = context.Request;

        //get session id
        var sessionId = httpContextAccessor?.HttpContext?.Request.Headers["TokenSessionId"] ?? "Default";

        //identify path for mvc call / api call
        var requestPath = httpContextAccessor?.HttpContext?.Request.Path.ToString().ToLower();

        //get list of path for mvc
        //string[] mvcPath = configuration.GetSection("MvcRequestPath").Get<string[]>();
        var mvcPath = SettingsConfigHelper.AppSetting<string[]>("ApplicationSettings", "MvcRequestPath");

        // Get stack trace for the exception with source file information
        var st = new StackTrace(exception, true);
        // Get the top stack frame
        var frame = st.GetFrame(0);
        // Get the line number from the stack frame
        int line = 0;
        string? controller = string.Empty;
        string? action = string.Empty;
        IReadOnlyDictionary<string, object>? routeValues = ((dynamic)context.Request).RouteValues as IReadOnlyDictionary<string, object>;

        if (frame != null)
        {
            line = frame.GetFileLineNumber();
            controller = routeValues?.Count() > 0 ? routeValues["controller"].ToString() : "";
            action = routeValues?["action"]?.ToString() ?? "N/A";
        }

        message = exception.GetExceptionMessages(exception.Message);

        string? userId = httpContextAccessor?.HttpContext?.GetHeader(ContextKeys.UserId);
        string? clientId = httpContextAccessor?.HttpContext?.GetHeader(ContextKeys.ClientId);
        string? userName = httpContextAccessor?.HttpContext?.GetHeader(ContextKeys.UserName);
        string? clientName = httpContextAccessor?.HttpContext?.GetHeader(ContextKeys.ClientName);

        //}

        var exceptionAt = new ExceptionOnMethod { MethodName = action, ClassName = controller, AtLineNo = line };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        IHttpContextAccessor httpContextAccessor1 = new HttpContextAccessor();
        var clientResponse = new ResponseDto<object>(httpContextAccessor1)
        {
            Status = Status.Failed,
            Message = message,
            MessageType = MessageType.Exception,
            Exception = new Exception(exception.StackTrace),
            StatusCode = context.Response.StatusCode,
            ExceptionRaisedOn = exceptionAt,
            DateTime = DateTime.Now,
            EmailStatus = true,
            Id = userId,
            ClientId = clientId,
            ClientName = clientName,
            UserName = userName,
            Log = trace.GetTraceLogs($"<strong>Exception Code: {exceptionType} Exception Message: </strong> {message} <br> {exceptionAt}")
        };

        MailRequest mailRequest = new()
        {
            UserId = userId,
            Message = clientResponse.Message,
            Subject = clientResponse.MessageType,
            EmailType = EmailNotificationType.EXCEPTION,
            ToEmail = new[] { SettingsConfigHelper.AppSetting("ApplicationSettings", "SendExceptionEmailTo:Email") },
            CCEmail = Array.Empty<string>(),
            Body = _emailService.GenerateExceptionBody(clientResponse, userId),
            ClientId = clientId,
            ClientName = clientName,
            UserName = userName
        };

        //sending email to cc user for exception
        new Thread(() =>
        {
            Thread.CurrentThread.IsBackground = true;
            _emailService.SendEmailAsync(mailRequest);
        }).Start();

        if (mvcPath == null)
        {
            //clientResponse.Message = "Something went wrong! Please contact support team!";
            await context.Response.WriteAsync(JsonSerializer.Serialize(clientResponse, options));

        }
        else
        if (mvcPath.Contains(requestPath))
        {
            var path = context.Request.PathBase;
            if (context.Response.StatusCode == 404)
            {
                path = context.Request.Path = path + "/Home/Home/Page404";
            }
            else if (context.Response.StatusCode == 403)
            {
                path = context.Request.Path = path + "/Home/Home/AccessDenied";
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var url = context.Request.Headers["Referer"];//Url for go back
                var redirectUrl = string.Format("/Home/Home/Error?url={0}&errorMessage={1}&errorCodes={2}", url, message, 500);

                Uri baseUrl = new Uri($"{context.Request.Scheme}://{context.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}{redirectUrl}");
                myUri = baseUrl;
                path = httpContextAccessor.HttpContext.Request.PathBase;
                //                context.Response.Redirect(redirectUrl);
            }
            //context.Response.Redirect(context.Request.Path + "?errorTitle={exception.Message}");
            context.Response.Redirect(myUri.AbsoluteUri);
        }
    }


}
//public static class com {
//    /// <summary>
//    /// Converts the provided app-relative path into an absolute Url containing the 
//    /// full host name
//    /// </summary>
//    /// <param name="relativeUrl">App-Relative path</param>
//    /// <returns>Provided relativeUrl parameter as fully qualified Url</returns>
//    /// <example>~/path/to/foo to http://www.web.com/path/to/foo</example>
//    public static string ToAbsoluteUrl(this string relativeUrl,IHttpContextAccessor httpContextAccessor)
//    {
//        if (string.IsNullOrEmpty(relativeUrl))
//            return relativeUrl;

//        if (httpContextAccessor.HttpContext..Current == null)
//            return relativeUrl;

//        if (relativeUrl.StartsWith("/"))
//            relativeUrl = relativeUrl.Insert(0, "~");
//        if (!relativeUrl.StartsWith("~/"))
//            relativeUrl = relativeUrl.Insert(0, "~/");

//        var url = HttpContext.Current.Request.Url;
//        var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

//        return String.Format("{0}://{1}{2}{3}",
//            url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
//    }
//}



//public class GlobalErrorHandlingMiddleware
//{
//    //private readonly RequestDelegate _next;
//    //private readonly ITrace _trace = new TraceRepository();
//    //internal readonly bool IsTracingRequired;
//    ////private string _controller = string.Empty;
//    ////private string _action = string.Empty;

//    //public GlobalErrorHandlingMiddleware()
//    //{
//    //    //_next = next;

//    //    IsTracingRequired = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
//    //}
//    //public async Task Invoke(HttpContext context)
//    //{
//    //    try
//    //    {
//    //        //_controller = "";
//    //        //_action = "";

//    //        await _next(context);
//    //    }
//    //    catch (Exception ex)
//    //    {
//    //        await HandleExceptionAsync(context, ex, IsTracingRequired);
//    //    }
//    //}
//    public static Task HandleExceptionAsync(HttpContext context,
//        Exception exception)
//    {
//        HttpStatusCode status;
//        string message;
//        var exceptionType = exception.GetType();

//        var request = context.Request;

//        // Get stack trace for the exception with source file information
//        var st = new StackTrace(exception, true);
//        // Get the top stack frame
//        var frame = st.GetFrame(0);
//        // Get the line number from the stack frame
//        int line = 0;
//        string? controller = string.Empty;
//        string? action = string.Empty;

//        if (frame != null)
//        {
//            line = frame.GetFileLineNumber();
//            //var currentUser = context.Username.Identity.Name;
//            controller = request.RouteValues["controller"]?.ToString();
//            action = request.RouteValues["action"]?.ToString();

//        }

//        if (exceptionType == typeof(BadRequestException))
//        {
//            message = exception.Message;
//            status = HttpStatusCode.BadRequest;
//        }
//        else if (exceptionType == typeof(NotFoundException))
//        {
//            message = exception.Message;
//            status = HttpStatusCode.NotFound;
//        }
//        else if (exceptionType == typeof(System.NotImplementedException))
//        {
//            status = HttpStatusCode.NotImplemented;
//            message = exception.Message;
//        }
//        else if (exceptionType == typeof(System.UnauthorizedAccessException))
//        {
//            status = HttpStatusCode.Unauthorized;
//            message = exception.Message;
//        }
//        else if (exceptionType == typeof(System.Collections.Generic.KeyNotFoundException))
//        {
//            status = HttpStatusCode.Unauthorized;
//            message = exception.Message;
//        }
//        else
//        {
//            status = HttpStatusCode.InternalServerError;
//            message = exception.Message;
//        }
//        var exceptionResult = JsonSerializer.Serialize(new ResponseDto<object>()
//        {
//            RecordStatus = RecordStatus.Failed,
//            Message = message,
//            MessageType = MessageType.Exception,
//            Exception = new Exception(exception.StackTrace),
//            StatusCode = context.ResponseDto.StatusCode,
//            ExceptionRaisedOn = { MethodName = action, ClassName = controller, AtLineNo = line }

//        });
//        context.ResponseDto.ContentType = CommonMessages.ApplicationJson;
//        context.ResponseDto.StatusCode = (int)status;
//        return context.ResponseDto.WriteAsync(exceptionResult);
//    }
//}