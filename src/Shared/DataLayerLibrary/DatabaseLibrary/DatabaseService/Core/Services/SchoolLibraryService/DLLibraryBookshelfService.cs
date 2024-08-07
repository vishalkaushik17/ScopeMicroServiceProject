using DataBaseServices.Core.Contracts.SchoolLibraryContracts;
using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.Persistence.Models.School.Library;
namespace DataBaseServices.Core.Services.SchoolLibraryService;


public sealed class DLLibraryBookshelfService : BaseGenericRepository<LibraryBookshelfModel>,
    IDataLayerLibraryBookshelfContract

{

    #region Constructor
    public DLLibraryBookshelfService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext, ITrace trace) :
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
    public async Task<List<LibraryBookshelfModel>?> GetAllAsync(string rackId)
    {

        var modelRecordsFromDb = await _dbContext.LibraryBookshelves.Where(m => m.RecordStatus == EnumRecordStatus.Active && m.RackId == rackId).ToListAsync();
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
    public async Task<LibraryBookshelfModel?> GetAsync(string BookshelfId, string rackId)
    {
        var record = await _dbContext.LibraryBookshelves.FirstOrDefaultAsync
            (m => m.RecordStatus == EnumRecordStatus.Active & m.Id == BookshelfId && m.RackId == rackId);
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


    public async Task<bool> FindExistingRecordAsync(string name, string? BookshelfId, string rackId)
    {
        var records = await _dbContext.LibraryBookshelves.Where(m => m.RecordStatus == EnumRecordStatus.Active && m.Id == BookshelfId && m.RackId == rackId).ToListAsync();
        //if no records inserted in db or cache
        if (records.Count == 0)
            return false;

        if (string.IsNullOrWhiteSpace(BookshelfId))
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
                                                         && m.Id != BookshelfId));
        }
    }

    #region Add Method

    /// <summary>
    /// Add LibraryBookshelf record
    /// </summary>
    /// <param name="modelRecord">model object</param>
    /// <returns>return model object</returns>
    public async Task<LibraryBookshelfModel> AddAsync(LibraryBookshelfModel modelRecord)
    {
        //Save data to db
        await _dbContext.LibraryBookshelves.AddAsync(modelRecord);
        await _dbContext.SaveChangesAsync();
        return await Task.Run(() => modelRecord);

    }
    #endregion Add Method

    #region Update Method
    public async Task<LibraryBookshelfModel> UpdateAsync(LibraryBookshelfModel modelRecord)
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
    #endregion Public Methods

    /// <summary>
    /// pending for implementation
    /// dont use rightnow
    /// </summary>
    /// <param name="bookId"></param>
    /// <returns></returns>
    public async Task<bool> IsRecordInUseAsync(string bookShelfId)
    {
        return await _dbContext.LibraryBookMasters.AnyAsync(m => m.RecordStatus == EnumRecordStatus.Active && m.LanguageId == bookShelfId);
    }
}