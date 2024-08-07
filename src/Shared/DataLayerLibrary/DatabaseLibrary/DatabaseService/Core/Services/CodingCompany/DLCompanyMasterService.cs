using DataBaseServices.Core.Contracts.CodingCompany;
using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.Master.Company;

namespace DataBaseServices.Core.Services.CodingCompany;

public sealed class DLCompanyMasterService : BaseGenericRepository<CompanyMasterModel>, IDataLayerCompanyMaster
{
    public DLCompanyMasterService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, ITrace trace) :
       base(dbContext, httpContextAccessor, trace)
    {
    }
    public async Task<CompanyMasterModel> AddAsync(CompanyMasterModel modelRecord)
    {
        modelRecord.Save(_userId);
        _dbContext.CompanyMasters.Add(modelRecord);
        await _dbContext.SaveChangesAsync();

        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "Retrieving newly generated Id from database.".MarkInformation());
        modelRecord = await _dbContext.CompanyMasters.Include(m => m.CompanyMasterProfile).FirstOrDefaultAsync(m => m.Name == modelRecord.Name &&
        m.SuffixDomain == modelRecord.SuffixDomain && m.Email == modelRecord.Email
        && m.CreatedOn == modelRecord.CreatedOn);

        return modelRecord;

    }

    public async Task<CompanyMasterModel?> GetAsync(string Id)
    {
        return await _dbContext.CompanyMasters.FirstOrDefaultAsync(m => m.Id == Id && m.RecordStatus == EnumRecordStatus.Active);
    }

    public async Task<List<CompanyMasterModel>> GetAllAsync()
    {
        return await _dbContext.CompanyMasters.Where(m => m.RecordStatus == EnumRecordStatus.Active).ToListAsync();
    }

    public async Task<IQueryable<CompanyMasterModel>> GetAllAsQuariableAsync()
    {
        return await Task.Run(() => _dbContext.CompanyMasters.Where(m => m.RecordStatus == EnumRecordStatus.Active).AsQueryable());
    }

    public async Task<CompanyMasterModel?> GetCompanyAsPerCompanyTypeIdAsync(string companyTypeId)
    {
        return await _dbContext.CompanyMasters.FirstOrDefaultAsync(model => model.CompanyTypeId == companyTypeId);

    }

    public async Task<ApplicationDbContext> SetDbContextAsync(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        return dbContext;
    }

    public async Task<CompanyMasterModel?> GetCompanyByReferenceCodeAsync(string referenceCode)
    {
        return await _dbContext.CompanyMasters.Include(m => m.CompanyMasterProfile).FirstOrDefaultAsync(m => m.ReferenceCode == referenceCode && m.RecordStatus == EnumRecordStatus.Active);
    }

    public async Task<CompanyMasterModel?> GetSchoolByDemoRequestIdAsync(string DemoRequestId)
    {
        return await _dbContext.CompanyMasters.FirstOrDefaultAsync(m => m.DemoRequestId == DemoRequestId && m.RecordStatus == EnumRecordStatus.Active);
    }

    public async Task<bool> GetSchoolInDemoModeAsync(string Id)
    {
        return await Task.Run(() => _dbContext.CompanyMasters.Any(model => model.Id == Id && !model.IsDemoExpired && model.IsDemoMode && model.RecordStatus == EnumRecordStatus.Active));
    }

    public async Task RemoveAsync(string Id)
    {
        var record = await GetAsync(Id);
        record?.Delete(_userId);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<CompanyMasterModel> UpdateAsync(CompanyMasterModel modelRecord)
    {
        modelRecord.Update(_userId);
        _dbContext.Entry(modelRecord).State = EntityState.Modified;

        await _dbContext.SaveChangesAsync();
        modelRecord.CheckIsEditable(_modificationInDays);
        return modelRecord;
    }
}
