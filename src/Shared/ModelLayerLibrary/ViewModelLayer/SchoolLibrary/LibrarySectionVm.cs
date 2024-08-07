using System.ComponentModel.DataAnnotations;

namespace VModelLayer.SchoolLibrary;

public class LibrarySectionVm : BaseVMTemplate
{
    /// <summary>
    /// Section View Model
    /// </summary>

    [Required(ErrorMessage = "Section name is required!")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select library to assign room!")]
    public string LibraryId { get; set; }

    public ICollection<LibraryHallVm> Libraries { get; set; }

    [Required(ErrorMessage = "Please select room to assign section!")]
    public string RoomId { get; set; }

    public ICollection<LibraryRoomVm> Rooms { get; set; }

    public LibrarySectionVm()
    {
        Libraries = new List<LibraryHallVm>();
        Rooms = new List<LibraryRoomVm>();
    }
}