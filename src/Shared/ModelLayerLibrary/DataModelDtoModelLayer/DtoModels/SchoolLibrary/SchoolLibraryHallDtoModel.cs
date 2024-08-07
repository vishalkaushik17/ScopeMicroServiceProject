using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.SchoolLibrary
{

    /// <summary>
    /// School Library Dto model
    /// </summary>
    public class SchoolLibraryHallDtoModel : BaseDtoTemplate
    {
        [Required(ErrorMessage = "Library name is required!")]
        [StringLength(50, ErrorMessage = "Provided value exceed the required limit (50 Characters).")]
        public string Name { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Provided value exceed the required limit (50 Characters).")]
        public string Location { get; set; } = "N/A";

        [StringLength(200, ErrorMessage = "Provided value exceed the required limit (200 Characters).")]
        public string Description { get; set; } = "N/A";
    }
}
