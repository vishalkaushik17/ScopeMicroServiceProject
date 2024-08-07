using DataBaseServices.Core.Contracts.EmployeeContracts;
using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.Persistence.Models.School.Employee;
using System.Transactions;
namespace DataBaseServices.Core.Services.CommonServices;

public sealed class DLEmployeeStudentParentService : BaseGenericRepository<EmployeeStudentParentModel>, IDataLayerEmployeeStudentParentContract

{

    #region Constructor
    public DLEmployeeStudentParentService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext, ITrace trace) :
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
    public async Task<List<EmployeeStudentParentModel>?> GetAllAsync()
    {

        var modelRecordsFromDb = await _dbContext.EmployeeStudentParentMaster.Where(m => m.RecordStatus == EnumRecordStatus.Active).ToListAsync();
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
    public async Task<EmployeeStudentParentModel?> GetAsync(string id)
    {
        var record = await _dbContext.EmployeeStudentParentMaster.FirstOrDefaultAsync
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

    #endregion Get Method


    public async Task<bool> FindExistingRecordAsync(string name, string? id)
    {
        var records = await _dbContext.EmployeeQualifications.Where(m => m.RecordStatus == EnumRecordStatus.Active).ToListAsync();
        //if no records inserted in db or cache
        if (records.Count == 0)
            return false;

        if (string.IsNullOrWhiteSpace(id))
        {
            //New Record

            return await Task.Run(() => records.Any(m => m.DegreeMaster.Name.ToUpper() == name.ToUpper()
                                                         && m.RecordStatus == EnumRecordStatus.Active));
        }
        else
        {
            //it will performed on update the record
            //Existing record
            return await Task.Run(() => records.Any(m => m.DegreeMaster.Name.ToUpper() == name.ToUpper()
                                                         && m.RecordStatus == EnumRecordStatus.Active
                                                         && m.Id != id));
        }
    }

    #region Add Method


    public async Task<EmployeeStudentParentModel> AddAsync(EmployeeStudentParentModel modelRecord)
    {
        using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {

            try
            {

                AddressModel permanentAddress = new AddressModel();
                AddressModel communicationAddress = new AddressModel();


                permanentAddress = modelRecord.PermanentAddress;
                communicationAddress = modelRecord.CommunicationAddress;

                permanentAddress.Save(_userId);
                communicationAddress.Save(_userId);

                await _dbContext.Addresses.AddAsync(permanentAddress);
                await _dbContext.Addresses.AddAsync(communicationAddress);
                await _dbContext.SaveChangesAsync();
                //getting address id
                permanentAddress = await _dbContext.Addresses.FirstOrDefaultAsync(m => m.Address1 == permanentAddress.Address1 && m.CreatedOn == permanentAddress.CreatedOn);
                communicationAddress = await _dbContext.Addresses.FirstOrDefaultAsync(m => m.Address1 == communicationAddress.Address1 && m.CreatedOn == communicationAddress.CreatedOn);

                if (permanentAddress == null || communicationAddress == null)
                {
                    transactionScope.Dispose();
                    throw new Exception("Issue with finding address!");
                }

                modelRecord.PermanentAddressId = permanentAddress.Id;
                modelRecord.CommunicationAddressId = communicationAddress.Id;
                modelRecord.Save(_userId);

                //stop adding nested address in table
                modelRecord.PermanentAddress = null;
                modelRecord.CommunicationAddress = null;


                //Save data to db
                await _dbContext.AddAsync(modelRecord);
                await _dbContext.SaveChangesAsync();

                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, "Retrieving newly generated Id from database.".MarkInformation());
                modelRecord = await _dbContext.EmployeeStudentParentMaster.FirstOrDefaultAsync(m => m.UIDNo == modelRecord.UIDNo &&
                    m.CreatedOn == modelRecord.CreatedOn);

                if (modelRecord == null)
                {
                    transactionScope.Dispose();
                    throw new Exception("Issue with saving data!");
                }

                return await Task.Run(() => modelRecord);
            }
            catch (Exception ex)
            {
                transactionScope.Dispose();
                await ex.SendExceptionMailAsync();
                throw new Exception(ex.Message);
            }
        }

    }
    #endregion Add Method

    #region Update Method
    public async Task<EmployeeStudentParentModel> UpdateAsync(EmployeeStudentParentModel modelRecord)
    {
        try
        {
            modelRecord.Update(_userId);

            _dbContext.Entry(modelRecord).State = EntityState.Modified;

            //All operation done from business class, so only save the record.
            await _dbContext.SaveChangesAsync();

            return await Task.Run(() => modelRecord);

        }
        catch (Exception ex)
        {
            await ex.SendExceptionMailAsync();
            throw new Exception(ex.Message);
        }
    }
    #endregion Update Method

    #region Delete Method
    public async Task<EmployeeStudentParentModel> DeleteAsync(EmployeeStudentParentModel modelRecord)
    {
        try
        {

            modelRecord.Delete(_userId);

            _dbContext.Entry(modelRecord).State = EntityState.Modified;
            //All operation done from business class, so only save the record.
            await _dbContext.SaveChangesAsync();

            return await Task.Run(() => modelRecord);
        }
        catch (Exception ex)
        {
            await ex.SendExceptionMailAsync();
            throw new Exception(ex.Message);
        }
    }
    #endregion Delete Method
    #endregion Public Methods
    #region Private Methods



    public async Task<bool> IsRecordInUseAsync(string id)
    {
        return await _dbContext.Addresses.AnyAsync(m => m.RecordStatus == EnumRecordStatus.Active && m.Id == id);
    }
    public void SetCurrentDataContext(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public ApplicationDbContext GetCurrentDataContext()
    {
        return _dbContext;
    }

    #endregion Private Methods

}