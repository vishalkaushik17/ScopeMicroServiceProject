using ModelTemplates.DtoModels.BaseDtoContract;
using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.DtoModels.SchoolLibrary
{
    /// <summary>
    /// Media Type Dto model
    /// </summary>
    public class LibraryMediaTypeDtoModel : BaseDtoTemplate
    {
        [Required(ErrorMessage = "MediaType name is required!")]
        [StringLength(30, ErrorMessage = "Provided value exceed the required limit (30 Characters).")]
        public string Name { get; set; } = string.Empty;

    }
}
