using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.SchoolLibrary;

/// <summary>
/// Room Dto model
/// </summary>
public class LibraryRoomDtoModel : BaseDtoTemplate
{
    [Required(ErrorMessage = "Room name is required!")]
    [StringLength(30, ErrorMessage = "Provided value exceed the required limit (30 Characters).")]
    public string Name { get; set; } = string.Empty;

    public SchoolLibraryHallDtoModel? Library { get; set; }

    [Required(ErrorMessage = "Please select library to assign room!")]
    public string LibraryId { get; set; }

}
