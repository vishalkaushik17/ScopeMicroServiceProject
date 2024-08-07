using GenericFunction;
using GenericFunction.Constants.Authorization;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Mvc;
using ModelTemplates.RequestNResponse;
using ScopeCoreWebApp.Methods;
using ScopeCoreWebApp.Middleware;
using SharedLibrary.Services;
using VModelLayer;
namespace ScopeCoreWebApp.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [AuthorizeByRole]
    public class HomeController : BaseAppController
    {
        //private readonly IConfiguration _configuration;
        public HomeController(IWebHostEnvironment hostingEnvironment, IConfiguration configuration,
            ITrace trace, IHttpContextAccessor httpContextAccessors, ICurrentUserService currentUserService
            , IStateManagement session) : base(trace, httpContextAccessors, currentUserService, session, hostingEnvironment)
        {
        }

        //default index page

        public async Task<ActionResult> IndexDashboard()
        {
            TempData["ExecutionLogs"] = _httpContextAccessor.HttpContext.Session.GetString("ExecutionLogs");
            _httpContextAccessor.HttpContext.Items.Remove("ExecutionLogs");


            return await Task.Run(() => View("Index"));
        }

        // GET: HomeController

        public async Task<SPAResponse<string>> Index()
        {
            try
            {
                BaseVMTemplate model = new BaseVMTemplate();
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

        [AuthorizeByRole(RoleName.AsstHR)]
        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
