using GenericFunction;
using GenericFunction.Constants.Authorization;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Mvc;
using ModelTemplates.RequestNResponse;
using ScopeCoreWebApp.Methods;
using ScopeCoreWebApp.Middleware;
using SharedLibrary.Services;
using VModelLayer;
using VModelLayer.EmployeeModule;

namespace ScopeCoreWebApp.Areas.EmployeeModule.Controllers
{
    [Area(nameof(EmployeeModule))]
    public class BankController : BaseAppController
    {
        public BankController(IWebHostEnvironment hostingEnvironment, ITrace trace, IHttpContextAccessor httpContextAccessors,
                ICurrentUserService currentUserService, IStateManagement session) : base(trace, httpContextAccessors, currentUserService, session, hostingEnvironment)
        {
        }

        [AuthorizeByRole(RoleName.HR, RoleName.AsstHR, RoleName.Owner, RoleName.Accountants, RoleName.Administrator, RoleName.Master, RoleName.Management, RoleName.ErpAdmin)]
        public async Task<SPAResponse<string>> Index()
        {
            try
            {
                BaseVMTemplate model = new BankVM();
                model.Id = "";

                _sPAResponse.HtmlResponse = await ControllerExtensions.RenderViewAsync(this, this.ControllerContext.ActionDescriptor.ActionName, model, true);
                AddLogPhase1(IsJsModuleAvailable: true, IsCssModuleAvailable: false);
            }
            catch (Exception ex)
            {
                await ex.SendExceptionMailAsync();
                CatchLogPhase(ex);
            }
            return await Task.Run(() => FinalLogPhase());

        }
        [AuthorizeByRole(RoleName.HR, RoleName.AsstHR, RoleName.Master, RoleName.ErpAdmin, RoleName.Administrator)]
        public IActionResult New()
        {
            return View("Form");
        }
    }
}
