using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.SchoolLibrary;

/// <summary>
/// Rack Dto model
/// </summary>
public class LibraryRackDtoModel : BaseDtoTemplate
{
    [Required(ErrorMessage = "Rack name is required!")]
    [StringLength(30, ErrorMessage = "Provided value exceed the required limit (30 Characters).")]
    public string Name { get; set; } = string.Empty;

    public SchoolLibraryHallDtoModel? Library { get; set; }

    [Required(ErrorMessage = "Please select library to assign Rack!")]
    public string LibraryId { get; set; }

    public LibraryRoomDtoModel? Room { get; set; }

    [Required(ErrorMessage = "Please select room to assign Rack!")]
    public string RoomId { get; set; }

    public LibrarySectionDtoModel Section { get; set; }

    [Required(ErrorMessage = "Please select section to assign Rack!")]
    public string SectionId { get; set; }




}