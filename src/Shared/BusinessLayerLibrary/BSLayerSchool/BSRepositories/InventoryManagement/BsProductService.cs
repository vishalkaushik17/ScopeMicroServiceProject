﻿using AutoMapper;
using BSLayerSchool.BaseClass;
using BSLayerSchool.BSInterfaces.InventoryManagement;
using DataCacheLayer.CacheRepositories.Interfaces;
using GenericFunction;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using MCInventoryLayer.Interface;
using Microsoft.AspNetCore.Http;
using ModelTemplates.DtoModels.Inventory;
using ModelTemplates.Persistence.Models.School.Inventory;
using static GenericFunction.CommonMessages;
namespace BSLayerSchool.BSRepositories.InventoryManagement
{
    public class BsProductService : BaseBusinessLayer, IBsProductContract
    {
        private readonly IMCProductContract _cacheService;

        public BsProductService(IMCProductContract cacheService, ICacheContract _cache, IHttpContextAccessor httpContextAccessor, IMapper iMapper, ITrace trace)
        : base(iMapper, httpContextAccessor, trace, _cache)
        {
            _cacheService = cacheService;
            _expirationTime = _applicationSettings.ModuleCacheSettings.InventoryModule.GetKeyLifeForCacheStorage();

        }

        /// <summary>
        /// Get record
        /// </summary>
        /// <param name="id">record id</param>
        /// <param name="UseCache">true for use Redis cache, default is true</param>
        /// <returns>ResponseDto object</returns>

        public async Task<ResponseDto<ProductDtoModel>> Get(string id, bool UseCache = true)
        {

            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started".MarkInformation());

            ProductModel? modelRecord = await _cacheService.GetCacheAsync(id, _modificationInDays, _clientId, UseCache);

            if (modelRecord == null)
            {
                return FailureResponse<ProductDtoModel>(CommonMessages.RecordNotFound);
            }


            //Send response to controller class
            return await Task.Run(() => SuccessResponse<ProductModel, ProductDtoModel>(id, modelRecord));
        }

        public async Task<ResponseDto<List<ProductDtoModel>>> GetAll(int pageNo, int pageSize,
            bool UseCache = true)
        {
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started".MarkInformation());

            //First check cache entry
            List<ProductModel>? modelRecords = new List<ProductModel>();

            modelRecords = await _cacheService.GetAllCacheAsync(_modificationInDays, _clientId, UseCache);


            if (modelRecords == null || modelRecords?.Count() == 0)
            {
                return FailureResponse<List<ProductDtoModel>>(CommonMessages.RecordNotFound);
            }

            return await Task.Run(() => SuccessResponse<ProductModel, List<ProductDtoModel>>(modelRecords));
        }

        public async Task<ResponseDto<ProductDtoModel>> AddAsync(ProductDtoModel dto, bool UseCache = true)
        {
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for adding new record!".MarkInformation());

            //find record from repository
            //check record availability by name
            if (await FindExistingRecordAsync(dto.ProductName, dto.Id, _modificationInDays, _clientId, UseCache))
            {
                return DuplicateEntryResponse<ProductDtoModel>("Product Name", dto.ProductName);
            }

            //Do all business operation here
            var modelRecord = new ProductModel();

            //assigning uploaded image to ProductImage 
            dto.ProductImage = dto.UploadImage.ChangeImageSize(150, 120);

            modelRecord = _mapper.Map<ProductModel>(dto);

            modelRecord = await _cacheService.AddCacheAsync(modelRecord, _clientId, UseCache);
            if (string.IsNullOrWhiteSpace(modelRecord.Id))
            {
                return FailureResponse<ProductDtoModel>(CommonMessages.OperationFailed);
            }
            modelRecord.CheckIsEditable(_modificationInDays);

            return await Task.Run(() => SuccessResponse<ProductModel, ProductDtoModel>(modelRecord.Id, modelRecord));
        }

        public async Task<ResponseDto<ProductDtoModel>> UpdateAsync(string id, ProductDtoModel dto,
            bool UseCache = true)
        {
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for updating record!".MarkInformation());

            //check record availability by name
            if (await FindExistingRecordAsync(dto.ProductName, dto.Id, _modificationInDays, _clientId, UseCache))
            {
                return DuplicateEntryResponse<ProductDtoModel>("Product Name", dto.ProductName);
            }

            //check record availability by id
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), $"Checking records for id {id}");
            ProductModel? modelRecord = await _cacheService.GetCacheAsync(id, _modificationInDays, _clientId, UseCache);

            //When no record available.
            if (modelRecord == null || modelRecord.Id != dto.Id)
            {
                return FailureResponse<ProductDtoModel>(CommonMessages.RecordNotFound);
            }
            //when no permission for updating record
            else if (modelRecord.IsEditable == false)
            {
                //watch.Stop();
                return FailureResponse<ProductDtoModel>(CommonMessages.NotPermitted);
            }
            //assigning uploaded image to ProductImage 
            var newImage = dto?.UploadImage?.ChangeImageSize(150, 120);
            dto.ProductImage = newImage == null ? dto.ProductImage : newImage;

            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), $"Record found updating!");
            modelRecord = _mapper.Map<ProductModel>(dto);
            await _cacheService.UpdateCacheAsync(modelRecord, _clientId, UseCache);
            modelRecord.CheckIsEditable(_modificationInDays);
            return await Task.Run(() => SuccessResponse<ProductModel, ProductDtoModel>(modelRecord.Id, modelRecord));


        }

        public async Task<ResponseDto<ProductDtoModel>> DeleteAsync(string id, bool UseCache = true)
        {
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for deleting record!".MarkInformation());

            //check record availability by id
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), $"Checking records for id {id}");
            ProductModel modelRecord = await _cacheService.GetCacheAsync(id, _modificationInDays, _clientId, UseCache);

            if (modelRecord == null)
            {
                return FailureResponse<ProductDtoModel>(CommonMessages.RecordNotFound);
            }
            else if (!modelRecord.IsEditable)
            {
                FailureResponse<ProductDtoModel>(CommonMessages.NotPermitted);
            }
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), $"Record found for marking it deleted!");

            //check if record is used in other table
            bool recordUsed = await _cacheService.IsRecordInUseAsync(id);//.AnyAsync(m => m.LibraryId == id);

            if (recordUsed)
            {
                return FailureResponse<ProductDtoModel>(CommonMessages.RecordInUsed);
            }

            modelRecord.ProductImage = null;

            modelRecord.Delete(_userId);

            modelRecord.RecordStatus = await _cacheService.DeleteCacheAsync(id, modelRecord, _modificationInDays, UseCache);


            if (modelRecord.RecordStatus != GenericFunction.Enums.EnumRecordStatus.Deleted)
            {
                return FailureResponse<ProductDtoModel>(CommonMessages.OperationFailed);
            }

            return await Task.Run(() => SuccessResponse<ProductModel, ProductDtoModel>(modelRecord.Id, modelRecord));

        }

        /// <summary>
        /// Finding existing records as per record name.
        /// </summary>
        /// <param name="fieldName">Name of field on which this record get compared.</param>
        /// <param name="id">Id of the record.</param>
        /// <param name="_modificationInDays">In how many days this record get modifying.</param>
        /// <param name="clientId">Client Id</param>
        /// <param name="useCache">boolean type - true mean use the cache server.  False will bypass cache and hit database directly.</param>
        /// <returns>If record found it return true.</returns>

        private async Task<bool> FindExistingRecordAsync(string fieldName, string? id, int _modificationInDays, string _clientId, bool useCache)
        {
            List<ProductModel>? records = await _cacheService.GetAllCacheAsync(_modificationInDays, _clientId, useCache);
            //if no records inserted in db or cache
            if (records == null || records?.Count == 0)
                return false;

            //id is null while adding new record, don't change the condition
            if (string.IsNullOrWhiteSpace(id))
            {
                //New Record

                return await Task.Run(() => records.Any(m => m.ProductName.ToUpper() == fieldName.ToUpper()
                                                             && m.RecordStatus == EnumRecordStatus.Active));
            }
            else
            {
                //when record id is available it mean we are updating record, so checking 
                //condition for existing record.
                return await Task.Run(() => records.Any(m => m.ProductName.ToUpper() == fieldName.ToUpper()
                                                             && m.RecordStatus == EnumRecordStatus.Active
                                                             && m.Id != id));
            }
        }


    }
}

