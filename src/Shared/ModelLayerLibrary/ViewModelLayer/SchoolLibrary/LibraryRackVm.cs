using System.ComponentModel.DataAnnotations;

namespace VModelLayer.SchoolLibrary;

public class LibraryRackVm : BaseVMTemplate
{
    /// <summary>
    /// Rack View Model
    /// </summary>

    [Required(ErrorMessage = "Rack name is required!")]
    public string Name { get; set; } = string.Empty;


    public ICollection<LibraryHallVm>? Libraries { get; set; }

    [Required(ErrorMessage = "Please select library to assign Rack!")]
    public string LibraryId { get; set; }



    public ICollection<LibraryRoomVm>? Rooms { get; set; }
    [Required(ErrorMessage = "Please select room to assign Rack!")]
    public string RoomId { get; set; }

    public List<LibrarySectionVm>? Sections { get; set; }

    [Required(ErrorMessage = "Please select section to assign Rack!")]
    public string SectionId { get; set; }



    public LibraryRackVm()
    {
        Libraries = new List<LibraryHallVm>();
        Sections = new List<LibrarySectionVm>();
        Rooms = new List<LibraryRoomVm>();

    }
}