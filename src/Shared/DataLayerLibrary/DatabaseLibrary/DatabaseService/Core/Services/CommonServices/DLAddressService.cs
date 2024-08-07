using DataBaseServices.Core.Contracts.CommonContracts;
using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.Persistence.Models.School.Employee;
using static GenericFunction.CommonMessages;
namespace DataBaseServices.Core.Services.CommonServices;

public sealed class DLAddressService : BaseGenericRepository<AddressModel>,
    IDataLayerAddressContract

{

    #region Constructor
    public DLAddressService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext, ITrace trace) :
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
    public async Task<List<AddressModel>?> GetAllAsync()
    {
        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess(), $"Process started - Reading records from database!");
        var modelRecordsFromDb = await _dbContext.Addresses.Where(m => m.RecordStatus == EnumRecordStatus.Active).ToListAsync();
        if (modelRecordsFromDb?.Count() > 0)
        {
            modelRecordsFromDb?.ToList().ForEach(
                u => u.IsEditable =
                    !u.IsEditable ? u.CreatedOn <= DateTime.Now && u.CreatedOn >= DateTime.Now.AddDays(-_modificationInDays) ? true : false : true
            );
        }
        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess(), $"Process started - Reading records from database completed!");

        return modelRecordsFromDb;

    }
    #endregion Get All Method

    #region Get Method
    public async Task<AddressModel?> GetAsync(string id)
    {
        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess(), $"Checking records for id {id}");

        var record = await _dbContext.Addresses.FirstOrDefaultAsync
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
        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationEnd.MarkProcess(), record != null ? record.Id : "No record found in database.");

        return record;

    }

    #endregion Get Method


    public async Task<bool> FindExistingRecordAsync(string name, string? id)
    {
        var records = await _dbContext.Addresses.Where(m => m.RecordStatus == EnumRecordStatus.Active).ToListAsync();
        //if no records inserted in db or cache
        if (records.Count == 0)
            return false;

        if (string.IsNullOrWhiteSpace(id))
        {
            //New Record

            return await Task.Run(() => records.Any(m => m.Address1.ToUpper() == name.ToUpper()
                                                         && m.RecordStatus == EnumRecordStatus.Active));
        }
        else
        {
            //it will performed on update the record
            //Existing record
            return await Task.Run(() => records.Any(m => m.Address1.ToUpper() == name.ToUpper()
                                                         && m.RecordStatus == EnumRecordStatus.Active
                                                         && m.Id != id));
        }
    }

    #region Add Method


    public async Task<AddressModel> AddAsync(AddressModel modelRecord)
    {
        //Save data to db
        await _dbContext.Addresses.AddAsync(modelRecord);
        await _dbContext.SaveChangesAsync();

        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "Retrieving newly generated Id from database.".MarkInformation());
        modelRecord = await _dbContext.Addresses.FirstOrDefaultAsync(m => m.Address1 == modelRecord.Address1 && m.CreatedOn == modelRecord.CreatedOn);

        return await Task.Run(() => modelRecord);

    }
    #endregion Add Method

    #region Update Method
    public async Task<AddressModel> UpdateAsync(AddressModel modelRecord)
    {
        _dbContext.Entry(modelRecord).State = EntityState.Modified;
        //All operation done from business class, so only save the record.
        await _dbContext.SaveChangesAsync();

        return await Task.Run(() => modelRecord);
    }
    #endregion Update Method

    #region Delete Method
    public async Task<AddressModel> DeleteAsync(AddressModel modelRecord)
    {
        _dbContext.Entry(modelRecord).State = EntityState.Modified;
        //All operation done from business class, so only save the record.
        await _dbContext.SaveChangesAsync();

        return await Task.Run(() => modelRecord);
    }
    #endregion Delete Method
    #endregion Public Methods
    #region Private Methods


    public async Task<bool> IsRecordInUseAsync(string id)
    {
        return await _dbContext.LibraryBookMasters.AnyAsync(m => m.RecordStatus == EnumRecordStatus.Active && m.Id == id);
    }


    #endregion Private Methods

}