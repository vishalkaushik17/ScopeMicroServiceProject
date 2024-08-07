using DataBaseServices.Core.Contracts.CodingCompany;
using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.EntityModels.Company;

namespace DataBaseServices.Core.Services.CodingCompany;

public sealed class DLCompanyTypeService : BaseGenericRepository<CompanyTypeModel>, IDataLayerCompanyType
{
    public DLCompanyTypeService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, ITrace trace) :
       base(dbContext, httpContextAccessor, trace)
    {
    }
    public async Task<CompanyTypeModel> Add(CompanyTypeModel modelRecord)
    {
        modelRecord.Save(_userId);
        _dbContext.CompanyTypes.Add(modelRecord);
        await _dbContext.SaveChangesAsync();

        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "Retrieving newly generated Id from database.".MarkInformation());
        modelRecord = await _dbContext.CompanyTypes.FirstOrDefaultAsync(m => m.TypeName == modelRecord.TypeName
        && m.CreatedOn == modelRecord.CreatedOn);

        return modelRecord;

    }


    public async Task<List<CompanyTypeModel>> AddRange(List<CompanyTypeModel> modelRecords)
    {
        foreach (var model in modelRecords)
        {
            model.Save(_userId);
        }

        await _dbContext.CompanyTypes.AddRangeAsync(modelRecords);
        await _dbContext.SaveChangesAsync();

        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "Retrieving newly generated Id from database.".MarkInformation());
        modelRecords = await _dbContext.CompanyTypes.ToListAsync();

        return modelRecords;

    }
    public async Task AddMultiple(CompanyTypeModel[] models)
    {
        _dbContext.CompanyTypes.AddRange(models);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<CompanyTypeModel?> Get(string Id)
    {
        return await _dbContext.CompanyTypes.FirstOrDefaultAsync(m => m.Id == Id && m.RecordStatus == EnumRecordStatus.Active);
    }

    public async Task<List<CompanyTypeModel>> GetAll()
    {
        return await _dbContext.CompanyTypes.Where(m => m.RecordStatus == EnumRecordStatus.Active).ToListAsync();
    }

    public async Task<CompanyTypeModel?> GetCompanyTypeAsPerEnum(string name)
    {
        return await _dbContext.CompanyTypes.FirstOrDefaultAsync(model =>
            model.TypeName == name);
    }

    public async Task Remove(string Id)
    {
        var record = await Get(Id);
        record?.Delete(_userId);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<CompanyTypeModel> Update(CompanyTypeModel modelRecord)
    {
        modelRecord.Update(_userId);

        await _dbContext.SaveChangesAsync();
        modelRecord.CheckIsEditable(_modificationInDays);
        return modelRecord;
    }
}
