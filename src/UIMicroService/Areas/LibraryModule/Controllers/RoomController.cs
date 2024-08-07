using Microsoft.AspNetCore.Mvc;
using ScopeCoreWebApp.Methods;
using VModelLayer.SchoolLibrary;

namespace ScopeCoreWebApp.Areas.LibraryModule.Controllers
{
    [Area("LibraryModule")]
    public class RoomController : Controller
    {
        [AuthorizeByRole]
        public IActionResult Index()
        {
            LibraryRoomVm model = new()
            {
                Id = "",
                Libraries = new List<LibraryHallVm>()
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
