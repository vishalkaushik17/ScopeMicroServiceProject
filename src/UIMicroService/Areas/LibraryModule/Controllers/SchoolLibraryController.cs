﻿using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Mvc;
using ModelTemplates.RequestNResponse;
using ScopeCoreWebApp.Methods;
using ScopeCoreWebApp.Middleware;
using SharedLibrary.Services;
using VModelLayer;
using VModelLayer.SchoolLibrary;
using static GenericFunction.ExtensionMethods;
namespace ScopeCoreWebApp.Areas.LibraryModule.Controllers
{
    [Area("LibraryModule")]
    public class SchoolLibraryController : BaseAppController
    {
        public SchoolLibraryController(IWebHostEnvironment hostingEnvironment, ITrace trace, IHttpContextAccessor httpContextAccessors,
            ICurrentUserService currentUserService, IStateManagement session) : base(trace, httpContextAccessors, currentUserService, session, hostingEnvironment)
        {
        }


        [AuthorizeByRole]
        public async Task<SPAResponse<string>> Index()
        {
            try
            {
                BaseVMTemplate model = new LibraryHallVm();
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
        [AuthorizeByRole]
        public IActionResult New()
        {
            return View("Form");
        }
    }
}
