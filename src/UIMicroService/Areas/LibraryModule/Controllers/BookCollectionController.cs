using Microsoft.AspNetCore.Mvc;
using ScopeCoreWebApp.Methods;
using VModelLayer.SchoolLibrary;

namespace ScopeCoreWebApp.Areas.LibraryModule.Controllers
{
    [Area("LibraryModule")]
    public class BookCollectionController : Controller
    {
        [AuthorizeByRole]
        public IActionResult Index()
        {
            LibraryBookCollectionVm model = new()
            {
                Id = "",
            };

            return View(model);
        }
        [AuthorizeByRole]
        public IActionResult New()
        {
            return View("Form");
        }
    }
}
