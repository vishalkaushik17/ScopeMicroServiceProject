using Microsoft.AspNetCore.Mvc;
using ScopeCoreWebApp.Methods;
using VModelLayer.SchoolLibrary;

namespace ScopeCoreWebApp.Areas.LibraryModule.Controllers
{
    [Area("LibraryModule")]
    public class BookshelfController : Controller
    {
        [AuthorizeByRole]
        public IActionResult Index()
        {
            LibraryBookshelfVm model = new()
            {
                Id = "",
                Libraries = new List<LibraryHallVm>(),
                Rooms = new List<LibraryRoomVm>(),
                Racks = new List<LibraryRackVm>(),
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
