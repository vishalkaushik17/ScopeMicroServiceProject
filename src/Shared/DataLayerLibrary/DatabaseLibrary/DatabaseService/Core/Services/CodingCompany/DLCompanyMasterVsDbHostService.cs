using DataBaseServices.Core.Contracts.CodingCompany;
using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.EntityModels.AppConfig;

namespace DataBaseServices.Core.Services.CodingCompany;

public sealed class DLCompanyMasterVsDbHostService : BaseGenericRepository<AppDBHostVsCompanyMaster>, IDataLayerCompanyMasterVsDbHost
{
    public DLCompanyMasterVsDbHostService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, ITrace trace) :
       base(dbContext, httpContextAccessor, trace)
    {
    }
    public async Task<AppDBHostVsCompanyMaster> Add(AppDBHostVsCompanyMaster modelRecord)
    {
        modelRecord.Save(_userId);
        _dbContext.AppDbHostVsCompanyMasters.Add(modelRecord);
        await _dbContext.SaveChangesAsync();

        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "Retrieving newly generated Id from database.".MarkInformation());
        modelRecord = await _dbContext.AppDbHostVsCompanyMasters
        .Include(m => m.CompanyMaster.CompanyMasterProfile)
        .Include(a => a.ApplicationHostMaster)
        .FirstOrDefaultAsync(m => m.AppHostId == modelRecord.AppHostId &&
        m.CompanyMasterId == m.CompanyMasterId
        && m.CreatedOn == modelRecord.CreatedOn);


        return modelRecord;

    }

    public async Task<AppDBHostVsCompanyMaster?> Get(string Id)
    {
        return await _dbContext.AppDbHostVsCompanyMasters.FirstOrDefaultAsync(m => m.Id == Id && m.RecordStatus == EnumRecordStatus.Active);
    }

    public async Task<List<AppDBHostVsCompanyMaster>> GetAll()
    {
        return await _dbContext.AppDbHostVsCompanyMasters.Include(m => m.CompanyMaster).Include(m => m.CompanyMaster.CompanyMasterProfile).Include(a => a.ApplicationHostMaster).Where(m => m.RecordStatus == EnumRecordStatus.Active).ToListAsync();
    }

    public async Task<IQueryable<AppDBHostVsCompanyMaster>> GetAllAsQuariable()
    {
        return await Task.Run(() => _dbContext.AppDbHostVsCompanyMasters.Where(m => m.RecordStatus == EnumRecordStatus.Active).AsQueryable());
    }

    public async Task<List<AppDBHostVsCompanyMaster>> GetAllWithProfileAndHost()
    {
        return await _dbContext.AppDbHostVsCompanyMasters.Include(m => m.CompanyMaster).Include(a => a.ApplicationHostMaster).ToListAsync();
    }
    public async Task<AppDBHostVsCompanyMaster?> GetAllWithProfileAndHost(string suffixDomain)
    {
        var domainList = await _dbContext.AppDbHostVsCompanyMasters.Include(m => m.CompanyMaster).Include(m => m.CompanyMaster.CompanyMasterProfile).Include(a => a.ApplicationHostMaster).Where(m => m.RecordStatus == EnumRecordStatus.Active).ToListAsync();
        return domainList.FirstOrDefault(x => x.CompanyMaster.SuffixDomain == suffixDomain);
        //_dbContext.AppDbHostVsCompanyMasters
        //                                .Include(m => m.CompanyMaster)
        //                                    .Include(m => m.CompanyMaster.CompanyMasterProfile)
        //                                    .Include(a => a.ApplicationHostMaster)
        //                                    .FirstOrDefaultAsync(m => m.AppHostId == dbDeploymentRecord.Id);
    }

    public async Task Remove(string Id)
    {
        var record = await Get(Id);
        record?.Delete(_userId);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<AppDBHostVsCompanyMaster> Update(AppDBHostVsCompanyMaster modelRecord)
    {
        modelRecord.Update(_userId);
        _dbContext.Entry(modelRecord).State = EntityState.Modified;

        await _dbContext.SaveChangesAsync();
        modelRecord.CheckIsEditable(_modificationInDays);
        return modelRecord;
    }
}
