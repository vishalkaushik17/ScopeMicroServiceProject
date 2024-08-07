using DataBaseServices.Core.Contracts.SchoolLibraryContracts;
using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.Persistence.Models.School.Library;
namespace DataBaseServices.Core.Services.SchoolLibraryService;


public sealed class DLLibraryAuthorService : BaseGenericRepository<LibraryAuthorModel>, IDataLayerLibraryAuthorContract

{

    #region Constructor
    public DLLibraryAuthorService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext, ITrace trace) :
        base(dbContext, httpContextAccessor, trace)
    {
        //No Implementations required. Everything is set in base class
    }

    #endregion Constructor

    #region Public Methods

    #region Get All Method
    /// <summary>
    /// Get all the records
    /// </summary>
    /// <param name="pageNo"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public async Task<List<LibraryAuthorModel>?> GetAllAsync()
    {

        var modelRecordsFromDb = await _dbContext.LibraryAuthors.Where(m => m.RecordStatus == EnumRecordStatus.Active).ToListAsync();
        if (modelRecordsFromDb?.Count() > 0)
        {
            modelRecordsFromDb?.ToList().ForEach(
                u => u.IsEditable =
                    !u.IsEditable ? u.CreatedOn <= DateTime.Now && u.CreatedOn >= DateTime.Now.AddDays(-_modificationInDays) ? true : false : true
            );
        }

        return modelRecordsFromDb;

    }
    #endregion Get All Method

    #region Get Method
    public async Task<LibraryAuthorModel?> GetAsync(string id)
    {
        var record = await _dbContext.LibraryAuthors.FirstOrDefaultAsync
            (m => m.RecordStatus == EnumRecordStatus.Active & m.Id == id);
        //as per preference value record get edited or deleted.
        //below condition will not get affected on get by id - single record.
        if (_modificationInDays > 0 && record != null)
        {
            var coDate = record.CreatedOn;
            IsRecordEditable = record.IsEditable;
            if (!IsRecordEditable)
            {
                IsRecordEditable = coDate <= DateTime.Now && coDate >= DateTime.Now.AddDays(-_modificationInDays) || record.IsEditable;
            }
            record.IsEditable = IsRecordEditable;
        }

        return record;

    }

    #endregion Get Method


    public async Task<bool> FindExistingRecordAsync(string name, string? id)
    {
        var records = await _dbContext.LibraryAuthors.Where(m => m.RecordStatus == EnumRecordStatus.Active).ToListAsync();
        //if no records inserted in db or cache
        if (records.Count == 0)
            return false;

        if (string.IsNullOrWhiteSpace(id))
        {
            //New Record

            return await Task.Run(() => records.Any(m => m.Name.ToUpper() == name.ToUpper()
                                                         && m.RecordStatus == EnumRecordStatus.Active));
        }
        else
        {
            //it will performed on update the record
            //Existing record
            return await Task.Run(() => records.Any(m => m.Name.ToUpper() == name.ToUpper()
                                                         && m.RecordStatus == EnumRecordStatus.Active
                                                         && m.Id != id));
        }
    }

    #region Add Method


    public async Task<LibraryAuthorModel> AddAsync(LibraryAuthorModel modelRecord)
    {
        //Save data to db
        await _dbContext.LibraryAuthors.AddAsync(modelRecord);
        await _dbContext.SaveChangesAsync();

        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "Retrieving newly generated Id from database.".MarkInformation());
        modelRecord = await _dbContext.LibraryAuthors.FirstOrDefaultAsync(m => m.Name == modelRecord.Name
        && m.CreatedOn == modelRecord.CreatedOn);

        return await Task.Run(() => modelRecord);

    }
    #endregion Add Method

    #region Update Method
    public async Task<LibraryAuthorModel> UpdateAsync(LibraryAuthorModel modelRecord)
    {
        _dbContext.Entry(modelRecord).State = EntityState.Modified;

        //All operation done from business class, so only save the record.
        await _dbContext.SaveChangesAsync();

        return await Task.Run(() => modelRecord);
    }
    #endregion Update Method

    #region Delete Method
    public async Task<bool> DeleteAsync()
    {
        bool isDeleted = false;
        await _dbContext.SaveChangesAsync();
        isDeleted = true;
        return await Task.Run(() => isDeleted);
    }
    #endregion Delete Method

    /// <summary>
    /// Check record for used in its reference tables.
    /// </summary>
    /// <param name="libraryId"></param>
    /// <returns></returns>
    public async Task<bool> IsRecordInUseAsync(string libraryId)
    {
        return await _dbContext.LibraryRooms.AnyAsync(m => m.RecordStatus == EnumRecordStatus.Active && m.LibraryId == libraryId);
    }
    #endregion Public Methods


}