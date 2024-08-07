using GenericFunction;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Mvc;
using ModelTemplates.RequestNResponse;
using ScopeCoreWebApp.Methods;
using ScopeCoreWebApp.Middleware;
using SharedLibrary.Services;
using VModelLayer;
using VModelLayer.SchoolLibrary;
using static GenericFunction.CommonMessages;
using static GenericFunction.ExtensionMethods;
namespace ScopeCoreWebApp.Areas.LibraryModule.Controllers
{
    [Area("LibraryModule")]
    public class BookMasterController : BaseAppController
    {
        private SPAResponse<string> _sPAResponse;
        public BookMasterController(IWebHostEnvironment hostingEnvironment, ITrace trace, IHttpContextAccessor httpContextAccessors,
            ICurrentUserService currentUserService, IStateManagement session) : base(trace, httpContextAccessors, currentUserService, session, hostingEnvironment)
        {
            _sPAResponse = new SPAResponse<string>(hostingEnvironment);
            _sPAResponse.Status = true;
            _sPAResponse.ResponseType = GenericFunction.Enums.SPAResponseType.HTML;
        }

        [AuthorizeByRole]
        public async Task<SPAResponse<string>> Index()
        {
            try
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess());
                _sPAResponse.FullViewName = $"{this.GetSupportedFileName(this.ControllerContext)}".ToLower();
                _sPAResponse.SupportedFileName = this.GetSupportedFileName(this.ControllerContext);
                _sPAResponse.ViewName = $"{this.GetActionMethod(this.ControllerContext)}".ToLower();
                BaseVMTemplate model = new LibraryAuthorVm();
                model.Id = "";

                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(),
                _isTracingRequired, $"Reading {this.GetFullViewPath(this.ControllerContext)} View.".GetCurrentLineNo());
                //_trace.TraceMe(ControllerContext.RouteData, _isTracingRequired, $"Reading {this.GetFullViewPath(this.ControllerContext)} View.".ToCss());
                _sPAResponse.HtmlResponse = await ControllerExtensions.RenderViewAsync(this, this.ControllerContext.ActionDescriptor.ActionName, model, true);
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(),
                _isTracingRequired, $"Reading completed! {this.GetFullViewPath(this.ControllerContext)} View.".GetCurrentLineNo());
                _sPAResponse.IsJsModuleAvailable = true;//load js module

                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(),
                _isTracingRequired, $"Creating version for js and css files required for : {this.GetFullViewPath(this.ControllerContext)} View.".GetCurrentLineNo());
                _sPAResponse.InitFileVersion();
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"Version added!".MarkProcess());


                if (string.IsNullOrWhiteSpace(_sPAResponse.HtmlResponse))
                {
                    _sPAResponse.MessageType = SPAMessageType.WAR;
                    _sPAResponse.Status = false;
                    _sPAResponse.ShowMessage = true;
                    _sPAResponse.Message = "View not available!!!";
                    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"{_sPAResponse.Message}: {this.GetFullViewPath(this.ControllerContext)}".MarkProcess(), _sPAResponse?.Status + " " + _sPAResponse?.Message);
                }
            }
            catch (Exception ex)
            {
                await ex.SendExceptionMailAsync();
                _sPAResponse.ShowMessage = true;
                _sPAResponse.MessageType = SPAMessageType.ERR;
                _sPAResponse.HtmlResponse = "";
                _sPAResponse.Status = false;
                _sPAResponse.Message = ex.Message;
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, $"{this.GetFullViewPath(this.ControllerContext)} - Exception occurred".MarkProcess(), _sPAResponse?.Status + " " + _sPAResponse?.Message);

            }
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess());
            _sPAResponse.Logs = _trace.GetTraceLogs("");
            return await Task.Run(() => _sPAResponse);

        }
        [AuthorizeByRole]
        public IActionResult New()
        {
            return View("Form");
        }
    }
}
