using DataBaseServices.Core.Contracts.CommonContracts;
using DataBaseServices.Persistence.BaseContract;
using DBOperationsLayer.Data.Context;
using GenericFunction;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelTemplates.Persistence.Models.School.Inventory;
using System.Transactions;
using static GenericFunction.CommonMessages;
namespace DataBaseServices.Core.Services.CommonServices;

public sealed class DLProductService : BaseGenericRepository<ProductModel>,
    IDataLayerProductContract

{

    #region Constructor
    public DLProductService(IHttpContextAccessor httpContextAccessors, ApplicationDbContext dbContext, ITrace trace) :
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
    public async Task<List<ProductModel>?> GetAllAsync()
    {
        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess(), $"Process started - Reading records from database!");
        var modelRecordsFromDb = await _dbContext.Products.Where(m => m.RecordStatus == EnumRecordStatus.Active).ToListAsync();
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
    public async Task<ProductModel?> GetAsync(string id)
    {
        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _isTracingRequired, OperationStart.MarkProcess(), $"Checking records for id {id}");

        ProductModel? record = await _dbContext.Products.FirstOrDefaultAsync
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
    public async Task<ProductModel> AddAsync(ProductModel modelRecord)
    {

        using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                modelRecord.Save(_userId);

                //stopping nested address record insertion
                await _dbContext.Products.AddAsync(modelRecord);
                await _dbContext.SaveChangesAsync();
                modelRecord = await _dbContext.Products.FirstOrDefaultAsync(m => m.ProductName == modelRecord.ProductName && m.CreatedOn == modelRecord.CreatedOn);

                //when no record found for above condition return back.
                if (modelRecord == null)
                {
                    transactionScope.Dispose();
                    return await Task.Run(() => new ProductModel());
                }

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
    public async Task<ProductModel> UpdateAsync(ProductModel modelRecord)
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
    public async Task<ProductModel> DeleteAsync(ProductModel modelRecord)
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
        return false;
        //return await _dbContext.LibraryBookMasters.AnyAsync(m => m.RecordStatus == EnumRecordStatus.Active && m.Id == id);
    }

    #endregion Private Methods

}