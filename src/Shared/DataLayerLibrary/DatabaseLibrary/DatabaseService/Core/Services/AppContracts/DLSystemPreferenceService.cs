using DataBaseServices.Core.Contracts.AppContracts;
using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.Persistence.Models.AppLevel;

namespace DataBaseServices.Core.Services.AppContracts;

public sealed class DLSystemPreferenceService : BaseGenericRepository<SystemPreferencesModel>, IDataLayerSystemPreferencesContract
{

    public DLSystemPreferenceService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, ITrace trace) :
        base(dbContext, httpContextAccessor, trace)
    {
    }

    public async Task<List<SystemPreferencesModel>> GetAllAsync()
    {
        return await _dbContext.SystemPreferences.ToListAsync();
    }
    public async Task<SystemPreferencesModel> AddAsync(SystemPreferencesModel modelRecord)
    {
        await _dbContext.SystemPreferences.AddAsync(modelRecord);
        await _dbContext.SaveChangesAsync();

        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "Retrieving newly generated Id from database.".MarkInformation());
        modelRecord = await _dbContext.SystemPreferences.FirstOrDefaultAsync(m => m.PreferenceName == modelRecord.PreferenceName && m.CreatedOn == modelRecord.CreatedOn);

        return modelRecord;
    }

    public Task<SystemPreferencesModel?> GetAsync(string id)
    {
        return _dbContext.SystemPreferences.FirstOrDefaultAsync(m => m.Id == id);

    }


    public async Task<bool> FindExistingRecordAsync(string name, string? id)
    {
        var records = await _dbContext.SystemPreferences.Where(m => m.RecordStatus == EnumRecordStatus.Active).ToListAsync();
        //if no records inserted in db or cache
        if (records.Count == 0)
            return false;

        if (string.IsNullOrWhiteSpace(id))
        {
            //New Record

            return await Task.Run(() => records.Any(m => m.PreferenceName.ToUpper() == name.ToUpper()
                                                         && m.RecordStatus == EnumRecordStatus.Active));
        }
        else
        {
            //it will performed on update the record
            //Existing record
            return await Task.Run(() => records.Any(m => m.PreferenceName.ToUpper() == name.ToUpper()
                                                         && m.RecordStatus == EnumRecordStatus.Active
                                                         && m.Id != id));
        }
    }

    public async Task<SystemPreferencesModel> UpdateAsync(SystemPreferencesModel modelRecord)
    {
        _dbContext.Entry(modelRecord).State = EntityState.Modified;

        //All operation done from business class, so only save the record.
        await _dbContext.SaveChangesAsync();

        return await Task.Run(() => modelRecord);
    }
    public async Task<bool> DeleteAsync()
    {
        bool isDeleted = false;
        await _dbContext.SaveChangesAsync();
        isDeleted = true;
        return await Task.Run(() => isDeleted);
    }
    public async Task<bool> IsRecordInUseAsync(string id)
    {
        return await _dbContext.SystemPreferences.AnyAsync(m => m.RecordStatus == EnumRecordStatus.Active && m.Id == id);
    }

}

