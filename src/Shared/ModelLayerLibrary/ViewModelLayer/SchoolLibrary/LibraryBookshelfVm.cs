using System.ComponentModel.DataAnnotations;

namespace VModelLayer.SchoolLibrary;

public class LibraryBookshelfVm : BaseVMTemplate
{
    /// <summary>
    /// Rack View Model
    /// </summary>

    [Required(ErrorMessage = "Bookshelf name is required!")]
    public string Name { get; set; } = string.Empty;


    public ICollection<LibraryRackVm>? Racks { get; set; }

    [Required(ErrorMessage = "Please select rack to assign bookshelf!")]
    public string RackId { get; set; }
    public ICollection<LibraryHallVm>? Libraries { get; set; }

    [Required(ErrorMessage = "Please select library to assign bookshelf!")]
    public string LibraryId { get; set; }



    public ICollection<LibraryRoomVm>? Rooms { get; set; }
    [Required(ErrorMessage = "Please select room to assign bookshelf!")]
    public string RoomId { get; set; }

    public List<LibrarySectionVm>? Sections { get; set; }

    [Required(ErrorMessage = "Please select section to assign bookshelf!")]
    public string SectionId { get; set; }



    public LibraryBookshelfVm()
    {
        Libraries = new List<LibraryHallVm>();
        Sections = new List<LibrarySectionVm>();
        Rooms = new List<LibraryRoomVm>();

    }
}