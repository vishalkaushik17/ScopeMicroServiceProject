using GenericFunction.Constants.Authorization;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScopeCoreWebApp.Methods;
using ScopeCoreWebApp.Middleware;
using SharedLibrary.Services;

namespace ScopeCoreWebApp.Areas.Home.Controllers
{
    [Area("Home")]
    [AllowAnonymous]
    public class HomeController : BaseAppController
    {
        public HomeController(IWebHostEnvironment hostingEnvironment,
            ITrace trace, IHttpContextAccessor httpContextAccessor, ICurrentUserService currentUserService, IStateManagement session)
            : base(trace, httpContextAccessor, currentUserService, session, hostingEnvironment)
        {
        }
        public IActionResult Index()
        {
            ViewBag.AddSlider = true;
            return View();
        }

        [AuthorizeByRole(RoleName.Master, RoleName.ErpAdmin, RoleName.Administrator)]
        public IActionResult Auth()
        {
            return View();
        }

        public ViewResult About()
        {
            ViewBag.AddSlider = false;
            return View();
        }

        public ViewResult ContactUs()
        {
            ViewBag.AddSlider = false;
            return View();
        }
        public ViewResult Clients()
        {
            ViewBag.AddSlider = false;
            return View();
        }
        public ViewResult Subscription()
        {
            ViewBag.AddSlider = false;
            return View();
        }
        public ViewResult Software()
        {
            ViewBag.AddSlider = false;
            return View();
        }

        public ViewResult test()
        {
            return View();
        }
        //public ViewResult Error()
        //{
        //    return View();
        //}
        //[Route ("Error")]
        [AllowAnonymous]
        public ViewResult Error(string url, string errorMessage, int errorCodes)
        {
            ViewBag.url = url;
            ViewBag.errorMessage = errorMessage;
            ViewBag.errorCodes = errorCodes;

            return View();
        }
        public ViewResult Page404()
        {
            return View();
        }
        public ViewResult AccessDenied()
        {
            ViewBag.AddSlider = false;
            return View();
        }

    }
}
