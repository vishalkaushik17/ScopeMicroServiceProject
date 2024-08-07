using ModelTemplates.Persistence.Models.School.Library;

namespace DataBaseServices.Core.Contracts.SchoolLibraryContracts
{
    /// <summary>
    /// For Managing School Library
    /// </summary>
    public interface IDataLayerLibraryRackContract
    {

        /// <summary>
        /// Return Rack record
        /// </summary>
        /// <param name="RackId">RackId</param>
        /// <param name="sectionId">sectionId</param>
        /// <returns>return record model</returns>
        Task<LibraryRackModel?> GetAsync(string RackId, string sectionId);

        /// <summary>
        /// Return all Rack records
        /// </summary>
        /// <param name="sectionId">sectionId for which Rack is going to return.</param>
        /// <returns></returns>
        Task<List<LibraryRackModel>?> GetAllAsync(string sectionId);

        /// <summary>
        /// Find record as per the Rack name and provided Rackid within section using sectionId.
        /// </summary>
        /// <param name="name">name to find in table.</param>
        /// <param name="RackId">RackId</param>
        /// <param name="sectionId">sectionId</param>
        /// <returns>return true if record is available as per the params.</returns>
        Task<bool> FindExistingRecordAsync(string name, string? RackId, string sectionId);

        /// <summary>
        /// Add new record to database.
        /// </summary>
        /// <param name="modelRecord">model record</param>
        /// <returns>return new updated record with id.</returns>
        Task<LibraryRackModel> AddAsync(LibraryRackModel modelRecord);

        /// <summary>
        /// Update record in database.
        /// </summary>
        /// <param name="modelRecord">Model record</param>
        /// <returns>return updated record.</returns>
        Task<LibraryRackModel> UpdateAsync(LibraryRackModel modelRecord);

        /// <summary>
        /// Delete record from database.
        /// </summary>
        /// <returns>return true if record successfully deleted.</returns>
        Task<bool> DeleteAsync();

        /// <summary>
        /// Check record is used in another table like Rack.
        /// </summary>
        /// <param name="rackId">Rack id</param>
        /// <returns>Return true is record is in use.</returns>
        Task<bool> IsRecordInUseAsync(string rackId);

    }

}
