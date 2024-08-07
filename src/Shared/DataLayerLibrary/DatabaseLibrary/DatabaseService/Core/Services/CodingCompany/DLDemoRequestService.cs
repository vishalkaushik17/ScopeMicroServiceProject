using DataBaseServices.Core.Contracts.CodingCompany;
using DataBaseServices.Core.Contracts.CommonServices;
using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction;
using GenericFunction.Enums;
using GenericFunction.GlobalService.EmailService.Contracts;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.Master.Company;
using Npgsql;
using System.Data;
namespace DataBaseServices.Core.Services.CodingCompany;


public sealed class DLDemoRequestService : BaseGenericRepository<DemoRequestModel>, IDataLayerDemoRequestContract

{

    private string? _origin;
    public IEmailService EmailService { get; }
    public IDataLayerApplicationHostContract _applicationHost { get; }

    public DLDemoRequestService(IDataLayerApplicationHostContract applicationHost,
        IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext,
        IEmailService emailService, ITrace trace) :
        base(dbContext, httpContextAccessor, trace)
    {
        //_origin = httpContextAccessor.Request.Headers["origin"];
        _origin = httpContextAccessor?.HttpContext?.Request?.Headers["origin"];
        EmailService = emailService;
        _applicationHost = applicationHost;
        base._dbContext = new ApplicationDbContext(dbContext._Options, httpContextAccessor, trace);

    }


    public async Task<DemoRequestModel?> GetNewDemoRequest(string demoId)
    {

        return await _dbContext.DemoRequests.FirstOrDefaultAsync(model => model.Id == demoId &&
            !model.IsDemoActivated && model.RecordStatus == EnumRecordStatus.Active);
    }

    public async Task<DemoRequestModel?> GetRecordByReferenceCode(string referenceCode)
    {

        return await _dbContext.DemoRequests.FirstOrDefaultAsync(m => m.ReferenceCode == referenceCode && m.RecordStatus == EnumRecordStatus.Active);
    }
    public async Task<List<DemoRequestModel>> GetAllByReferenceCode(string referenceCode)
    {
        return await _dbContext.DemoRequests.Where(m => m.ReferenceCode == referenceCode && m.RecordStatus == EnumRecordStatus.Active).ToListAsync();
    }

    public async Task<List<DemoRequestModel>> GetAll()
    {
        return await _dbContext.DemoRequests.Where(m => m.RecordStatus == EnumRecordStatus.Active).ToListAsync();
    }

    public async Task<DemoRequestModel?> GetDemoRequestById(string id)
    {
        return await _dbContext.DemoRequests.FirstOrDefaultAsync(m => m.Id == id && m.RecordStatus == EnumRecordStatus.Active);
    }

    public async Task<DemoRequestModel?> GetByAny(string? id = "", string? refCode = "", string? website = "", string? contactNo = "", string? email = "")
    {
        return await _dbContext.DemoRequests.FirstOrDefaultAsync(m => m.Id == id || m.ContactNo == contactNo || m.ReferenceCode == refCode || m.Email == email || m.Website == website && m.RecordStatus == EnumRecordStatus.Active);
    }

    public async Task<DemoRequestModel?> GetByEmailId(string emailId)
    {

        return await _dbContext.DemoRequests.FirstOrDefaultAsync(m => m.Email == emailId && m.RecordStatus == EnumRecordStatus.Active);

    }

    public async Task<DemoRequestModel?> GetByContactNo(string contactNo)
    {
        return await _dbContext.DemoRequests.FirstOrDefaultAsync(m => m.ContactNo == contactNo && m.RecordStatus == EnumRecordStatus.Active);
    }

    public async Task<DemoRequestModel?> GetByWebsite(string website)
    {
        return await _dbContext.DemoRequests.FirstOrDefaultAsync(m => m.Website == website && m.RecordStatus == EnumRecordStatus.Active);
    }

    public async Task<bool> CheckByReferenceCode(string referenceCode)
    {
        return await _dbContext.CompanyMasters.AnyAsync(model => model.ReferenceCode == referenceCode && model.RecordStatus == EnumRecordStatus.Active);
    }
    public async Task<DemoRequestModel> GenerateDemoRequest(DemoRequestModel model)
    {
        model.Save();
        await _dbContext.DemoRequests.AddAsync(model);
        await _dbContext.SaveChangesAsync();
        return model;
    }


    public bool createDatabase(string server, string port, string database, string username, string password)
    {
        string connectionstring = string.Format("Server = {0}; Port ={1}; Uid = {2}; Pwd = {3}; pooling = true; Allow Zero Datetime = False; Min Pool Size = 0; Max Pool Size = 200; ", server, port, username, password);
        using (var con = new NpgsqlConnection { ConnectionString = connectionstring })
        {
            using (var command = new NpgsqlCommand { Connection = con })
            {
                if (con.State == ConnectionState.Open)
                    con.Close();

                try
                {
                    con.Open();
                }
                catch (Exception ex)
                {
                    ex.SendExceptionMailAsync().GetAwaiter().GetResult();
                    return false;
                }

                try
                {
                    var s0 = $"CREATE DATABASE IF NOT EXISTS `{database}`;";
                    var cmd = new NpgsqlCommand(s0, con);
                    cmd.ExecuteNonQuery();

                    //command.CommandText = @"CREATE DATABASE IF NOT EXISTS @database";
                    //command.Parameters.AddWithValue("@database", database);
                    //command.ExecuteNonQuery();//Execute your command
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public async Task<DemoRequestModel?> GetByReferenceCode(string referenceCode)
    {
        return await _dbContext.DemoRequests.FirstOrDefaultAsync(model => model.ReferenceCode == referenceCode && model.RecordStatus == EnumRecordStatus.Active);
    }

    public async Task<List<DemoRequestModel>> GetAll(bool isActive)
    {
        return await _dbContext.DemoRequests.Where(model => model.RecordStatus == EnumRecordStatus.Active).ToListAsync();
    }

    public async Task<DemoRequestModel?> IsDemoAlreadyRequested(string website, string contactNo, string email)
    {
        return await _dbContext.DemoRequests.FirstOrDefaultAsync(model => (model.Email == email || model.ContactNo == contactNo ||
                                                                              model.Website == website) && model.RecordStatus == EnumRecordStatus.Active);
    }

    Task<DemoRequestModel?> IDataLayerDemoRequestContract.ActivateDemo(string demoId, string? customSuffixDomain, string hostId)
    {
        throw new NotImplementedException();
    }

    public async Task<DemoRequestModel> Update(DemoRequestModel modelRecord)
    {

        modelRecord.Update(_userId);

        _dbContext.Entry(modelRecord).State = EntityState.Modified;

        _dbContext.SaveChanges();
        modelRecord.CheckIsEditable(_modificationInDays);
        return modelRecord;
    }

    public async Task<DemoRequestModel> Add(DemoRequestModel model)
    {
        model.Save();
        await _dbContext.DemoRequests.AddAsync(model);
        await _dbContext.SaveChangesAsync();
        return model;
    }
}