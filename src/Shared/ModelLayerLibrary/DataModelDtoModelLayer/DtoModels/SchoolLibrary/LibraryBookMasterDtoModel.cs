using ModelTemplates.CustomValidations;
using ModelTemplates.DtoModels.BaseDtoContract;
using ModelTemplates.DtoModels.CommonDtoModels;
using ModelTemplates.DtoModels.Inventory;
using ModelTemplates.DtoModels.SchoolLibrary;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ModelTemplates.Persistence.Models.School.Library;

/// <summary>
/// Book master dto Model is derived by Generic template which comprises of
/// completed and incomplete methods for book dto Component
/// This Data Model is responsible to communicate between business logic 
/// and database table.
/// </summary>
public class LibraryBookMasterDtoModel : BaseDtoTemplate
{

    /// <summary>
    /// Name / Title of the book
    /// </summary>
    [Required(ErrorMessage = "Book title is required!")]
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Sub title of the book
    /// </summary>

    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string SubTitle { get; set; } = string.Empty;
    /// <summary>
    /// Text snippet for the book
    /// </summary>
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string Snippet { get; set; } = string.Empty;

    /// <summary>
    /// Book description.
    /// </summary>
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string Description { get; set; } = "n/a";

    /// <summary>
    /// Printed price
    /// </summary>
    [Required(ErrorMessage = "Book price is required!")]
    [Range(int.MinValue, 9999, ErrorMessage = "Pages range should be between 0 to 9999")]
    public double Price { get; set; } = 0;

    /// <summary>
    /// Total no of pages
    /// </summary>
    [Range(int.MinValue, 9999, ErrorMessage = "Pages range should be between 0 to 9999")]
    public int Pages { get; set; } = 0;

    /// <summary>
    /// Book thumbnail image
    /// </summary>
    [AllowNull]
    public string BookImage { get; set; } = string.Empty;

    /// <summary>
    /// Date on book is published.
    /// </summary>

    [ValidteDateLessThenTodayAttribute]
    public DateTime PublishedDate { get; set; } = DateTime.Now;



    /// <summary>
    /// ISBN code 10 format no.
    /// </summary>
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string ISBN10 { get; set; } = "n/a";

    /// <summary>
    /// ISBN code 13 format no.
    /// </summary>
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string ISBN13 { get; set; } = "n/a";

    /// <summary>
    /// Book volume no, eg. Volume 1,Volume 2...
    /// </summary>
    [StringLength(100, ErrorMessage = "Provided value exceed the required limit (100 Characters).")]
    public string VolumeNo { get; set; }


    public LanguageDtoModel Language { get; set; }
    public string LanguageId { get; set; }

    public CurrencyDtoModel Currency { get; set; }
    public string CurencyId { get; set; }

    public LibraryAuthorDtoModel Author { get; set; }
    public string AuthorId { get; set; }

    public VendorDtoModel Publisher { get; set; }
    public string PublisherId { get; set; }

    public LibraryBookCollectionDtoModel Collection { get; set; }
    public string CollectionId { get; set; }
}

