using ModelTemplates.Persistence.Models.School.Library;

namespace DataBaseServices.Core.Contracts.SchoolLibraryContracts
{
    /// <summary>
    /// For Managing School Library
    /// </summary>
    public interface IDataLayerLibrarySectionContract
    {


        /// <summary>
        /// Return Section record
        /// </summary>
        /// <param name="SectionId">SectionId</param>
        /// <param name="roomId">roomId</param>
        /// <returns>return record model</returns>
        Task<LibrarySectionModel?> GetAsync(string SectionId, string roomId);

        /// <summary>
        /// Return all Section records
        /// </summary>
        /// <param name="roomId">room id for which Section is going to return.</param>
        /// <returns></returns>
        Task<List<LibrarySectionModel>?> GetAllAsync(string roomId);

        /// <summary>
        /// Find record as per the Section name and provided Sectionid within room using roomId.
        /// </summary>
        /// <param name="name">name to find in table.</param>
        /// <param name="SectionId">SectionId</param>
        /// <param name="roomId">roomId</param>
        /// <returns>return true if record is available as per the params.</returns>
        Task<bool> FindExistingRecordAsync(string name, string? SectionId, string roomId);

        /// <summary>
        /// Add new record to database.
        /// </summary>
        /// <param name="modelRecord">model record</param>
        /// <returns>return new updated record with id.</returns>
        Task<LibrarySectionModel> AddAsync(LibrarySectionModel modelRecord);

        /// <summary>
        /// Update record in database.
        /// </summary>
        /// <param name="modelRecord">Model record</param>
        /// <returns>return updated record.</returns>
        Task<LibrarySectionModel> UpdateAsync(LibrarySectionModel modelRecord);

        /// <summary>
        /// Delete record from database.
        /// </summary>
        /// <returns>return true if record successfully deleted.</returns>
        Task<bool> DeleteAsync();

        /// <summary>
        /// Check record is used in another table like section.
        /// </summary>
        /// <param name="sectionId">Section id</param>
        /// <returns>Return true is record is in use in child.</returns>
        Task<bool> IsRecordInUseAsync(string sectionId);

    }

}
