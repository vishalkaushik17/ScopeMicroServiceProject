using GenericFunction;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using static GenericFunction.CommonMessages;
namespace UnitOfWork.DI
{
    public class UseCustomCORSOptionsMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly string[] _Headers = { "Origin", "X-Requested-With", "Content-Type", "DataType", "Accept", "school", "School", "Authorization", "application/json; charset=utf-8", "json" };
        private readonly string[] _Methods = { "POST", "GET", "OPTIONS" };
        private readonly bool _isTracingRequired;
        
        public UseCustomCORSOptionsMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            
            
            ITrace _trace = new TraceRepository(httpContextAccessor);
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(),
           _isTracingRequired, OperationStart.MarkProcess(), "Custom CORS Middleware Invoke Process started from : " + this.NameOfClass() + "".GetCurrentLineNo());

            return BeginInvoke(context, configuration, _trace);
        }

        private Task BeginInvoke(HttpContext context, IConfiguration configuration, ITrace _trace)
        {
            string[] hosts = configuration.GetSection("AllowDomains").Get<string[]>();


            //if (!context.Request.IsHttps)
            //{
            //    context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
            //    return context.Response.WriteAsync("Only https connection allowed!");

            //}

            if (!context.Request.Headers.Keys.Any(x => _Headers.Contains(x)) ||
                !_Methods.Contains(context.Request.Method))
            {
                context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(),
                _isTracingRequired, OperationEnd.MarkProcess(), $"Header not acceptable : {StatusCodes.Status406NotAcceptable}" + this.NameOfClass() + "".GetCurrentLineNo());

                return context.Response.WriteAsync("Header request Failed!");
            }

            ////it wont allow host which are going to access our system outside of the domain which are listed in appsettings.
            //if (!hosts.Contains(context.Connection.RemoteIpAddress.ToString().ToLower()))
            //{
            //    context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
            //    return context.Response.WriteAsync("Outside domain not allowed! " + context.Request.HttpContext.Connection.RemoteIpAddress);
            //}

            //context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { (string)context.Request.Headers["Origin"] });
            //context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Origin, X-Requested-With, Content-Type, Accept, School, school,Authorization, application/json; charset=utf-8, json" });
            //context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "POST, GET", "OPTIONS" });
            //context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(),
            _isTracingRequired, OperationEnd.MarkProcess(), "Custom CORS Middleware Process ended from : " + this.NameOfClass() + "".GetCurrentLineNo());
            return _next.Invoke(context);
        }
    }
}