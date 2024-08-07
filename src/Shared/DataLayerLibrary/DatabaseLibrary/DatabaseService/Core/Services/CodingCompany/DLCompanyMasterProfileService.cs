using DataBaseServices.Core.Contracts.CodingCompany;
using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.Master.Company;

namespace DataBaseServices.Core.Services.CodingCompany;

public sealed class DLCompanyMasterProfileService : BaseGenericRepository<CompanyMasterProfileModel>, IDataLayerCompanyMasterProfile
{
    public DLCompanyMasterProfileService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, ITrace trace) :
       base(dbContext, httpContextAccessor, trace)
    {
    }
    public async Task<CompanyMasterProfileModel> Add(CompanyMasterProfileModel modelRecord)
    {
        modelRecord.Save(_userId);
        _dbContext.CompanyMasterProfiles.Add(modelRecord);
        await _dbContext.SaveChangesAsync();

        modelRecord = await _dbContext.CompanyMasterProfiles.FirstOrDefaultAsync(m => m.Name == modelRecord.Name && m.HostName == modelRecord.HostName);
        return modelRecord;
    }

    public async Task<CompanyMasterProfileModel?> Get(string Id)
    {
        return await _dbContext.CompanyMasterProfiles.FirstOrDefaultAsync(m => m.Id == Id && m.RecordStatus == EnumRecordStatus.Active);
    }

    public async Task<List<CompanyMasterProfileModel>> GetAll()
    {
        return await _dbContext.CompanyMasterProfiles.Where(m => m.RecordStatus == EnumRecordStatus.Active).ToListAsync();
    }

    public async Task<IQueryable<CompanyMasterProfileModel>> GetAllAsQuariable()
    {
        return await Task.Run(() => _dbContext.CompanyMasterProfiles.Where(m => m.RecordStatus == EnumRecordStatus.Active).AsQueryable());
    }

    public async Task<CompanyMasterProfileModel?> GetCompanyAsPerCompanyTypeId(string companyTypeId)
    {
        return await _dbContext.CompanyMasterProfiles
            .FirstOrDefaultAsync(model => model.Id == companyTypeId && model.RecordStatus == EnumRecordStatus.Active);
    }

    public async Task Remove(string Id)
    {
        var record = await Get(Id);
        record?.Delete(_userId);
        await _dbContext.SaveChangesAsync();
    }



    public async Task<CompanyMasterProfileModel> Update(CompanyMasterProfileModel modelRecord)
    {
        modelRecord.Update(_userId);
        _dbContext.Entry(modelRecord).State = EntityState.Modified;

        await _dbContext.SaveChangesAsync();

        //Check model record editable status as per the preference value
        modelRecord.CheckIsEditable(_modificationInDays);

        return modelRecord;
    }
}
