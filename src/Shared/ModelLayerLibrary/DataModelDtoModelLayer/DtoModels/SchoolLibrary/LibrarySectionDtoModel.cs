using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.SchoolLibrary;

/// <summary>
/// Room Dto model
/// </summary>
public class LibrarySectionDtoModel : BaseDtoTemplate
{
    [Required(ErrorMessage = "Section name is required!")]
    [StringLength(30, ErrorMessage = "Provided value exceed the required limit (30 Characters).")]
    public string Name { get; set; } = string.Empty;

    public SchoolLibraryHallDtoModel? Library { get; set; }

    [Required(ErrorMessage = "Please select library to assign section!")]
    public string LibraryId { get; set; }

    public LibraryRoomDtoModel? Room { get; set; }

    [Required(ErrorMessage = "Please select room to assign section!")]
    public string RoomId { get; set; }





}