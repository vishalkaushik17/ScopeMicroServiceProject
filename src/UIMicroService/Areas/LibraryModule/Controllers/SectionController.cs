using Microsoft.AspNetCore.Mvc;
using ScopeCoreWebApp.Methods;
using VModelLayer.SchoolLibrary;

namespace ScopeCoreWebApp.Areas.LibraryModule.Controllers
{
    [Area("LibraryModule")]
    public class SectionController : Controller
    {
        [AuthorizeByRole]
        public IActionResult Index()
        {
            LibrarySectionVm model = new()
            {
                Id = "",
                Libraries = new List<LibraryHallVm>(),
                Rooms = new List<LibraryRoomVm>()
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
