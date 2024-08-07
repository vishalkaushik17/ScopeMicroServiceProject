using Microsoft.AspNetCore.Mvc;
using ScopeCoreWebApp.Methods;
using VModelLayer.SchoolLibrary;

namespace ScopeCoreWebApp.Areas.LibraryModule.Controllers
{
    [Area("LibraryModule")]
    public class RackController : Controller
    {
        [AuthorizeByRole]
        public IActionResult Index()
        {
            LibraryRackVm model = new()
            {
                Id = "",
                Libraries = new List<LibraryHallVm>(),
                Rooms = new List<LibraryRoomVm>(),
                Sections = new List<LibrarySectionVm>()
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
