﻿using ModelTemplates.Persistence.Models.School.Library;

namespace DataBaseServices.Core.Contracts.SchoolLibraryContracts
{
    /// <summary>
    /// For Managing School Library
    /// </summary>
    public interface IDataLayerLibraryMediaTypeContract
    {

        /// <summary>
        /// Return record
        /// </summary>
        /// <param name="id">record id</param>
        /// <returns></returns>
        Task<LibraryMediaTypeModel?> GetAsync(string id);

        /// <summary>
        /// Get all records.
        /// </summary>
        /// <returns>Get list of records.</returns>
        Task<List<LibraryMediaTypeModel>?> GetAllAsync();

        /// <summary>
        /// Find record by name.
        /// </summary>
        /// <param name="name">Name to find in table.</param>
        /// <param name="id">Id for checking record with found record by name.</param>
        /// <returns>Return true if record found.</returns>
        Task<bool> FindExistingRecordAsync(string name, string? id);

        /// <summary>
        /// Add new record to database.
        /// </summary>
        /// <param name="modelRecord">model record</param>
        /// <returns>return new updated record with id.</returns>
        Task<LibraryMediaTypeModel> AddAsync(LibraryMediaTypeModel modelRecord);

        /// <summary>
        /// Update record in database.
        /// </summary>
        /// <param name="modelRecord">Model record</param>
        /// <returns>return updated record.</returns>
        Task<LibraryMediaTypeModel> UpdateAsync(LibraryMediaTypeModel modelRecord);

        /// <summary>
        /// Delete record from database.
        /// </summary>
        /// <returns>return true if record successfully deleted.</returns>
        Task<bool> DeleteAsync();

        /// <summary>
        /// Check library record is used in another table like room
        /// </summary>
        /// <param name="id">Library id</param>
        /// <returns>Return true is record is in use.</returns>
        Task<bool> IsRecordInUseAsync(string id);

    }

}
