using DataBaseServices.Core.Contracts.SchoolLibraryContracts;
using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.Persistence.Models.School.Library;
namespace DataBaseServices.Core.Services.SchoolLibraryService;

public sealed class DLLibrarySectionService : BaseGenericRepository<LibrarySectionModel>, IDataLayerLibrarySectionContract

{
    #region Constructor
    public DLLibrarySectionService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext, ITrace trace) :
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
    public async Task<List<LibrarySectionModel>?> GetAllAsync(string roomId)
    {

        var modelRecordsFromDb = await _dbContext.LibrarySections.Where(m => m.RecordStatus == EnumRecordStatus.Active && m.RoomId == roomId).ToListAsync();
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
    public async Task<LibrarySectionModel?> GetAsync(string SectionId, string roomId)
    {
        var record = await _dbContext.LibrarySections.FirstOrDefaultAsync
            (m => m.RecordStatus == EnumRecordStatus.Active & m.Id == SectionId && m.RoomId == roomId);
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


    public async Task<bool> FindExistingRecordAsync(string name, string? SectionId, string roomId)
    {
        var records = await _dbContext.LibrarySections.Where(m => m.RecordStatus == EnumRecordStatus.Active && m.Id == SectionId && m.RoomId == roomId).ToListAsync();
        //if no records inserted in db or cache
        if (records.Count == 0)
            return false;

        if (string.IsNullOrWhiteSpace(SectionId))
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
                                                         && m.Id != SectionId));
        }
    }

    #region Add Method


    public async Task<LibrarySectionModel> AddAsync(LibrarySectionModel modelRecord)
    {
        //Save data to db
        await _dbContext.LibrarySections.AddAsync(modelRecord);
        await _dbContext.SaveChangesAsync();
        return await Task.Run(() => modelRecord);

    }
    #endregion Add Method

    #region Update Method
    public async Task<LibrarySectionModel> UpdateAsync(LibrarySectionModel modelRecord)
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
    public async Task<bool> IsRecordInUseAsync(string sectionId)
    {
        return await _dbContext.LibraryBookshelves.AnyAsync(m => m.RecordStatus == EnumRecordStatus.Active && m.SectionId == sectionId);
    }
}