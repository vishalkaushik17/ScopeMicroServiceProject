using ModelTemplates.Persistence.Models.School.Library;

namespace DataBaseServices.Core.Contracts.SchoolLibraryContracts
{
    /// <summary>
    /// For Managing School Library
    /// </summary>
    public interface IDataLayerLibraryBookshelfContract
    {

        /// <summary>
        /// Return Bookshelf record
        /// </summary>
        /// <param name="BookshelfId">BookshelfId</param>
        /// <param name="rackId">rackId</param>
        /// <returns>return record model</returns>
        Task<LibraryBookshelfModel?> GetAsync(string BookshelfId, string rackId);

        /// <summary>
        /// Return all Bookshelf records
        /// </summary>
        /// <param name="rackId">rackId for which Bookshelf is going to return.</param>
        /// <returns></returns>
        Task<List<LibraryBookshelfModel>?> GetAllAsync(string rackId);

        /// <summary>
        /// Find record as per the Bookshelf name and provided Bookshelfid within section using sectionId.
        /// </summary>
        /// <param name="name">name to find in table.</param>
        /// <param name="BookshelfId">BookshelfId</param>
        /// <param name="rackId">rackId</param>
        /// <returns>return true if record is available as per the params.</returns>
        Task<bool> FindExistingRecordAsync(string name, string? BookshelfId, string rackId);

        /// <summary>
        /// Add new record to database.
        /// </summary>
        /// <param name="modelRecord">model record</param>
        /// <returns>return new updated record with id.</returns>
        Task<LibraryBookshelfModel> AddAsync(LibraryBookshelfModel modelRecord);

        /// <summary>
        /// Update record in database.
        /// </summary>
        /// <param name="modelRecord">Model record</param>
        /// <returns>return updated record.</returns>
        Task<LibraryBookshelfModel> UpdateAsync(LibraryBookshelfModel modelRecord);

        /// <summary>
        /// Delete record from database.
        /// </summary>
        /// <returns>return true if record deleted.</returns>
        Task<bool> DeleteAsync();

        /// <summary>
        /// Check record is used in another table like Bookshelf.
        /// </summary>
        /// <param name="id">Bookshelf id</param>
        /// <param name="rackId">rackId</param>
        /// <returns>Return true is record is in use.</returns>
        Task<bool> IsRecordInUseAsync(string bookId);

    }

}
