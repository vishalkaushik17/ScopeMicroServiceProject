using DataBaseServices.Core.Contracts.CommonServices;
using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.DtoModels.AppConfig;
using ModelTemplates.EntityModels.AppConfig;
namespace DataBaseServices.Core.Services.CommonServices;

public sealed class DLApplicationHostService : BaseGenericRepository<ApplicationHostMasterModel>,
    IDataLayerApplicationHostContract

{

    #region Constructor
    public DLApplicationHostService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext, ITrace trace) :
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
    public async Task<List<ApplicationHostMasterModel>?> GetAllAsync()
    {

        var modelRecordsFromDb = await _dbContext.ApplicationHostMasters.Where(m => m.RecordStatus == EnumRecordStatus.Active).ToListAsync();
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
    public async Task<ApplicationHostMasterModel?> GetAsync(string id)
    {
        var record = await _dbContext.ApplicationHostMasters.FirstOrDefaultAsync
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

    public async Task<ApplicationHostMasterModel?> GetSpecificRecordAsync(string ConnectionString, DateTime createdOn)
    {
        var record = await _dbContext.ApplicationHostMasters.FirstOrDefaultAsync
            (m => m.RecordStatus == EnumRecordStatus.Active & m.ConnectionString == ConnectionString && m.CreatedOn == createdOn);
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
        var records = await _dbContext.ApplicationHostMasters.Where(m => m.RecordStatus == EnumRecordStatus.Active).ToListAsync();
        //if no records inserted in db or cache
        if (records.Count == 0)
            return false;

        if (string.IsNullOrWhiteSpace(id))
        {
            //New Record

            return await Task.Run(() => records.Any(m => m.Domain.ToUpper() == name.ToUpper()
                                                         && m.RecordStatus == EnumRecordStatus.Active));
        }
        else
        {
            //it will performed on update the record
            //Existing record
            return await Task.Run(() => records.Any(m => m.Domain.ToUpper() == name.ToUpper()
                                                         && m.RecordStatus == EnumRecordStatus.Active
                                                         && m.Id != id));
        }
    }

    #region Add Method
    //may be need to work here.
    public async Task<ApplicationHostMasterModel> AddHost(ApplicationHostMasterModel record, string username, string password, string hashString)
    {
        record.Save(_userId);
        _dbContext.ApplicationHostMasters.Add(record);
        await _dbContext.SaveChangesAsync();
        record = await _dbContext.ApplicationHostMasters.FirstOrDefaultAsync(m => m.UserName == username && m.Domain == record.Domain && m.DatabaseType == record.DatabaseType);
        return record;
    }

    public async Task<ApplicationHostMasterModel> AddAsync(ApplicationHostMasterModel modelRecord)
    {
        //Save data to db
        await _dbContext.AddAsync(modelRecord);
        await _dbContext.SaveChangesAsync();

        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "Retrieving newly generated Id from database.".MarkInformation());
        modelRecord = await _dbContext.ApplicationHostMasters.FirstOrDefaultAsync(m => m.ConnectionString == modelRecord.ConnectionString && m.CreatedOn == modelRecord.CreatedOn);

        return await Task.Run(() => modelRecord);

    }
    #endregion Add Method

    #region Update Method
    public async Task<ApplicationHostMasterModel> UpdateAsync(ApplicationHostMasterModel modelRecord)
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
    #region Private Methods

    public async Task<bool> IsRecordInUseAsync(string id)
    {
        return await _dbContext.ApplicationHostMasters.AnyAsync(m => m.RecordStatus == EnumRecordStatus.Active && m.Id == id);
    }

    public Task<ResponseDto<ApplicationHostMasterDtoModel>> GetJSON()
    {
        throw new NotImplementedException();
    }

    #endregion Private Methods

}
