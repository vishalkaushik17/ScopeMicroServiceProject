using ModelTemplates.Persistence.Models.School.Library;

namespace DataBaseServices.Core.Contracts.SchoolLibraryContracts
{
    /// <summary>
    /// For Managing School Library
    /// </summary>
    public interface IDataLayerLibraryRoomContract
    {

        /// <summary>
        /// Return room record
        /// </summary>
        /// <param name="roomId">roomId</param>
        /// <param name="libraryId">libraryId</param>
        /// <returns>return record model</returns>
        Task<LibraryRoomModel?> GetAsync(string roomId, string libraryId);

        /// <summary>
        /// Return all room records
        /// </summary>
        /// <param name="libraryId">Library id for which room is going to return.</param>
        /// <returns>return all records with given library id</returns>
        Task<List<LibraryRoomModel>?> GetAllAsync(string libraryId);

        /// <summary>
        /// Return all room records
        /// </summary>
        /// <returns>return all records</returns>
        Task<List<LibraryRoomModel>?> GetAll();

        /// <summary>
        /// Find record as per the room name and provided roomid within library using libraryId.
        /// </summary>
        /// <param name="name">name to find in table.</param>
        /// <param name="roomId">roomId</param>
        /// <param name="libraryid">LibraryId</param>
        /// <returns>return true if record is available as per the params.</returns>
        Task<bool> FindExistingRecordAsync(string name, string? roomId, string libraryid);

        /// <summary>
        /// Add new record to database.
        /// </summary>
        /// <param name="modelRecord">model record</param>
        /// <returns>return new updated record with id.</returns>
        Task<LibraryRoomModel> AddAsync(LibraryRoomModel modelRecord);

        /// <summary>
        /// Update record in database.
        /// </summary>
        /// <param name="modelRecord">Model record</param>
        /// <returns>return updated record.</returns>
        Task<LibraryRoomModel> UpdateAsync(LibraryRoomModel modelRecord);

        /// <summary>
        /// Delete record from database.
        /// </summary>
        /// <returns>return true if record successfully deleted.</returns>
        Task<bool> DeleteAsync();

        /// <summary>
        /// Check record is used in another table like section.
        /// </summary>
        /// <param name="roomId">roomId</param>
        /// <returns>Return true is record is in use.</returns>
        Task<bool> IsRecordInUseAsync(string roomId);

    }

}
