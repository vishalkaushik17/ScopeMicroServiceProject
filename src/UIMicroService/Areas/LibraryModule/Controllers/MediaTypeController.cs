using Microsoft.AspNetCore.Mvc;
using ScopeCoreWebApp.Methods;
using VModelLayer.SchoolLibrary;

namespace ScopeCoreWebApp.Areas.LibraryModule.Controllers
{
    [Area("LibraryModule")]
    public class MediaTypeController : Controller
    {
        [AuthorizeByRole]
        public IActionResult Index()
        {
            MediaTypeVm model = new MediaTypeVm();
            model.Id = "";
            return View(model);
        }
        [AuthorizeByRole]
        public IActionResult New()
        {
            return View("Form");
        }
    }
}
