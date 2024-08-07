using GenericFunction;
using GenericFunction.Enums;
using ModelTemplates.Core.GenericModel;
using ModelTemplates.DtoModels.SchoolLibrary;

namespace ModelTemplates.Persistence.Component.School.Library;

/// <summary>
/// School Library Component is derived by SchoolLibraryTemplate which comprises of
/// completed and incomplete methods for SchoolLibrary Component
/// This Data Model is responsible to communicate between business logic 
/// and database table.
/// </summary>
public class SchoolLibraryHallModel : BaseTemplate//, ISchoolLibraryContract
{
    /// <summary>
    /// Name of the library where section wise medias are allotted in different location school campus.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = "N/A";
    public string Description { get; set; } = "N/A";

    /// <summary>
    /// Default abstract method implementation for Saving library record.
    /// </summary>
    /// <param name="userId"></param>
    public new void Save(string userId)
    {
        base.Save(userId);
        Name = Name.ToCamelCase();
        Location = String.IsNullOrWhiteSpace(Location) ? "N/A" : Location;
        Description = String.IsNullOrWhiteSpace(Description) ? "N/A" : Description;
    }
}


