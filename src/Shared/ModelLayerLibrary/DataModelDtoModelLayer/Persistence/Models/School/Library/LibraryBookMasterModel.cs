using GenericFunction;
using ModelTemplates.Core.GenericModel;
using ModelTemplates.Persistence.Models.School.CommonModels;

namespace ModelTemplates.Persistence.Models.School.Library;

/// <summary>
/// Book master Model is derived by Generic template which comprises of
/// completed and incomplete methods for book master Component
/// This Data Model is responsible to communicate between business logic 
/// and database table.
/// </summary>
public class LibraryBookMasterModel : BaseTemplate
{

    /// <summary>
    /// Name / Title of the book
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Sub title of the book
    /// </summary>
    public string SubTitle { get; set; } = string.Empty;
    /// <summary>
    /// Text snippet for the book
    /// </summary>
    public string Snippet { get; set; } = string.Empty;

    /// <summary>
    /// Book description.
    /// </summary>
    public string Description { get; set; } = "n/a";

    /// <summary>
    /// Printed price
    /// </summary>
    public double Price { get; set; } = 0;

    /// <summary>
    /// Total no of pages
    /// </summary>
    public int Pages { get; set; } = 0;

    /// <summary>
    /// Book thumbnail image
    /// </summary>
    public string BookImage { get; set; } = string.Empty;

    /// <summary>
    /// Date on book is published.
    /// </summary>
    public DateTime PublishedDate { get; set; }



    /// <summary>
    /// ISBN code 10 format no.
    /// </summary>
    public string ISBN10 { get; set; } = "n/a";

    /// <summary>
    /// ISBN code 13 format no.
    /// </summary>
    public string ISBN13 { get; set; } = "n/a";

    /// <summary>
    /// Book volume no, eg. Volume 1,Volume 2...
    /// </summary>
    public string VolumeNo { get; set; }


    public LanguageModel Language { get; set; }
    public string LanguageId { get; set; }

    public CurrencyModel Currency { get; set; }
    public string CurrencyId { get; set; }

    public LibraryAuthorModel Author { get; set; }
    public string AuthorId { get; set; }

    public VendorModel Publisher { get; set; }
    public string PublisherId { get; set; }

    public LibraryBookCollectionModel Collection { get; set; }
    public string CollectionId { get; set; }
    /// <summary>
    /// Default save method, which will set Room record as per logged in UserId
    /// </summary>
    /// <param name="userId">Logged in User Id</param>
    public new void Save(string userId)
    {
        base.Save(userId);
        Title = Title.ToCamelCase().RemoveSpaces();
        SubTitle = SubTitle.ToCamelCase().RemoveSpaces();
        Description = Description.RemoveSpaces();
        Snippet = Snippet.RemoveSpaces();
        ISBN10 = ISBN10.RemoveSpaces();
        ISBN13 = ISBN13.RemoveSpaces();
        VolumeNo = VolumeNo.RemoveSpaces();

    }
}

