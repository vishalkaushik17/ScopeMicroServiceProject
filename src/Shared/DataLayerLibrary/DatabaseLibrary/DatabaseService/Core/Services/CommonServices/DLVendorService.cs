using DataBaseServices.Core.Contracts.CommonContracts;
using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.Persistence.Models.School.CommonModels;
using ModelTemplates.Persistence.Models.School.Employee;
using System.Transactions;
using static GenericFunction.CommonMessages;
namespace DataBaseServices.Core.Services.CommonServices;

public sealed class DLVendorService : BaseGenericRepository<VendorModel>,
    IDataLayerVendorContract

{

    #region Constructor
    public DLVendorService(IHttpContextAccessor httpContextAccessors, ApplicationDbContext dbContext, ITrace trace) :
        base(dbContext, httpContextAccessors, trace)
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
    public async Task<List<VendorModel>?> GetAllAsync()
    {
        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess(), $"Process started - Reading records from database!");
        var modelRecordsFromDb = await _dbContext.Vendors.Include(s => s.Address).Where(m => m.RecordStatus == EnumRecordStatus.Active).ToListAsync();
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
    public async Task<VendorModel?> GetAsync(string id)
    {
        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess(), $"Checking records for id {id}");

        VendorModel? record = await _dbContext.Vendors.Include(s => s.Address).FirstOrDefaultAsync
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


    #region Add Method

    /// <summary>
    /// Add new record to database.
    /// </summary>
    /// <param name="modelRecord">model record</param>
    /// <returns>return newly inserted record with id.</returns>
    public async Task<VendorModel> AddAsync(VendorModel modelRecord)
    {

        using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {

                AddressModel modelRecordForAddress = new AddressModel();

                //making address record separate for insertion 
                modelRecordForAddress = modelRecord.Address;

                modelRecordForAddress.Save(_userId);
                await _dbContext.Addresses.AddAsync(modelRecordForAddress);
                await _dbContext.SaveChangesAsync();

                //fetching newly added record with newly generated id.
                modelRecordForAddress = await _dbContext.Addresses.FirstOrDefaultAsync(m => m.Address1 == modelRecordForAddress.Address1 && m.CreatedOn == modelRecordForAddress.CreatedOn);

                //when no record found for above condition return back.
                if (modelRecordForAddress == null)
                {
                    transactionScope.Dispose();
                    return await Task.Run(() => new VendorModel());
                }

                //assigning newly generated address id to modelRecord.AddressId for foreign key reference.
                modelRecord.Save(modelRecordForAddress.Id, _userId);

                //stopping nested address record insertion
                modelRecord.Address = null;

                modelRecord.Save(modelRecordForAddress.Id, _userId);
                await _dbContext.Vendors.AddAsync(modelRecord);
                await _dbContext.SaveChangesAsync();


                modelRecord = await _dbContext.Vendors.FirstOrDefaultAsync(m => m.ContactNo == modelRecord.ContactNo && m.CreatedOn == modelRecord.CreatedOn);


                //when no record found for above condition return back.
                if (modelRecord == null)
                {
                    transactionScope.Dispose();
                    return await Task.Run(() => new VendorModel());
                }

                //reassigning address to modelRecord.
                modelRecord.Address = modelRecordForAddress;


                transactionScope.Complete();
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
    public async Task<VendorModel> UpdateAsync(VendorModel modelRecord)
    {
        using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {

            try
            {
                //for updating existing vendor record
                modelRecord.AddressId = modelRecord.AddressId;

                //for updating existing address record
                modelRecord.Address.Id = modelRecord.AddressId;

                //updating address record
                AddressModel addressModel = modelRecord.Address;


                addressModel.Save(_userId);
                addressModel.Update(_userId);

                _dbContext.Entry(addressModel).State = EntityState.Modified;

                modelRecord.Address = null;
                _dbContext.Entry(modelRecord).State = EntityState.Modified;

                modelRecord.Update(modelRecord.AddressId, _userId);
                //All operation done from business class, so only save the record.
                await _dbContext.SaveChangesAsync();
                transactionScope.Complete();
                modelRecord.Address = addressModel;
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
    #endregion Update Method

    #region Delete Method
    public async Task<VendorModel> DeleteAsync(VendorModel modelRecord)
    {
        using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {

            try
            {
                modelRecord.AddressId = modelRecord.AddressId;
                modelRecord.Address.Id = modelRecord.AddressId;


                //updating address record
                AddressModel addressModel = modelRecord.Address;
                addressModel.Delete(_userId);
                _dbContext.Entry(addressModel).State = EntityState.Modified;

                modelRecord.Address = null;
                modelRecord.Delete(_userId);

                _dbContext.Entry(modelRecord).State = EntityState.Modified;
                //All operation done from business class, so only save the record.
                await _dbContext.SaveChangesAsync();
                transactionScope.Complete();
                modelRecord.Address = addressModel;
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
    #endregion Delete Method
    #endregion Public Methods
    #region Private Methods


    public async Task<bool> IsRecordInUseAsync(string id)
    {
        return false;
        //return await _dbContext.LibraryBookMasters.AnyAsync(m => m.RecordStatus == EnumRecordStatus.Active && m.Id == id);
    }

    #endregion Private Methods

}