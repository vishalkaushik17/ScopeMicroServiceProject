using GenericFunction;
using ModelTemplates.Core.GenericModel;
using ModelTemplates.DtoModels.SchoolLibrary;
using ModelTemplates.Persistence.Component.School.Library;

namespace ModelTemplates.Persistence.Models.School.Library;

/// <summary>
/// Section Model is derived by Generic template which comprises of
/// completed and incomplete methods for section Component
/// This Data Model is responsible to communicate between business logic 
/// and database table.
/// Section belongs to room
/// </summary>
public class LibrarySectionModel : BaseTemplate
{
    /// <summary>
    /// Name of the media Room like book/cd/dvd/charts for school library
    /// </summary>
    public string Name { get; set; } = string.Empty;

    public LibraryRoomModel Room { get; set; }
    public string RoomId { get; set; }
    public SchoolLibraryHallModel Library { get; set; }
    public string LibraryId { get; set; }

    /// <summary>
    /// Default save method, which will set Room record as per logged in UserId
    /// </summary>
    /// <param name="userId">Logged in User Id</param>
    public new void Save(string userId)
    {
        base.Save(userId);
        Name = Name.ToCamelCase();
        LibraryId = LibraryId;
        RoomId = RoomId;
    }
}

