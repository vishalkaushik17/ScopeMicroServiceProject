using System.ComponentModel.DataAnnotations;

namespace VModelLayer.SchoolLibrary;

public class LibraryRoomVm : BaseVMTemplate
{
    /// <summary>
    /// Room View Model model
    /// </summary>

    [Required(ErrorMessage = "Room name is required!")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select library to assign room!")]
    public string LibraryId { get; set; }

    public ICollection<LibraryHallVm> Libraries { get; set; }

    public LibraryRoomVm()
    {
        Libraries = new List<LibraryHallVm>();
    }
}