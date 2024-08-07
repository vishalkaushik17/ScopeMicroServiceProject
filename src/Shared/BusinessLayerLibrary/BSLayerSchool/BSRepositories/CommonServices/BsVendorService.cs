using AutoMapper;
using BSLayerSchool.BaseClass;
using BSLayerSchool.BSInterfaces.CommonContracts;
using DataCacheLayer.CacheRepositories.Interfaces;
using GenericFunction;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using MCCommonLayer.Interface;
using Microsoft.AspNetCore.Http;
using ModelTemplates.DtoModels.Inventory;
using ModelTemplates.Persistence.Models.School.CommonModels;
using static GenericFunction.CommonMessages;
namespace BSLayerSchool.BSRepositories.CommonServices
{
    public sealed class BsVendorService : BaseBusinessLayer, IBsVendorContract
    {
        private readonly IMCVendorContract _cacheService;
        private readonly IMCAddressContract _cacheServiceAddress;
        public BsVendorService(IMCVendorContract cacheService, IMCAddressContract cacheServiceAddress, ICacheContract _cache, IHttpContextAccessor httpContextAccessor, IMapper iMapper, ITrace trace)
        : base(iMapper, httpContextAccessor, trace, _cache)
        {
            _cacheService = cacheService;
            _cacheServiceAddress = cacheServiceAddress;
            //_dlService = dlService;
            //_dlServiceForAddress = dlServiceForAddress;
            _expirationTime = _applicationSettings.ModuleCacheSettings.CommonModule.GetKeyLifeForCacheStorage();
        }

        /// <summary>
        /// Get record
        /// </summary>
        /// <param name="id">record id</param>
        /// <param name="UseCache">true for use Redis cache, default is true</param>
        /// <returns>ResponseDto object</returns>

        public async Task<ResponseDto<VendorDtoModel>> Get(string id, bool UseCache = true)
        {

            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started".MarkInformation());

            VendorModel? modelRecord = await _cacheService.GetCacheAsync(id, _modificationInDays, _clientId, UseCache);

            if (modelRecord == null)
            {
                return FailureResponse<VendorDtoModel>(CommonMessages.RecordNotFound);
            }


            //Send response to controller class
            return await Task.Run(() => SuccessResponse<VendorModel, VendorDtoModel>(id, modelRecord));
        }

        public async Task<ResponseDto<List<VendorDtoModel>>> GetAll(int pageNo, int pageSize,
            bool UseCache = true)
        {

            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started".MarkInformation());

            //First check cache entry
            List<VendorModel>? modelRecords = new List<VendorModel>();

            modelRecords = await _cacheService.GetAllCacheAsync(_modificationInDays, _clientId, UseCache);


            if (modelRecords == null || modelRecords?.Count() == 0)
            {
                return FailureResponse<List<VendorDtoModel>>(CommonMessages.RecordNotFound);
            }

            return await Task.Run(() => SuccessResponse<VendorModel, List<VendorDtoModel>>(modelRecords));
        }


        public async Task<ResponseDto<VendorDtoModel>> AddAsync(VendorDtoModel dto, bool UseCache = true)
        {
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for adding new record!".MarkInformation());

            var modelRecord = new VendorModel();

            modelRecord = _mapper.Map<VendorModel>(dto);

            //check record availability by name
            if (await FindExistingRecordAsync(dto.ContactNo, dto.Id, _modificationInDays, _clientId, UseCache))
            {
                return DuplicateEntryResponse<VendorDtoModel>("Contact No", dto.ContactNo);
            }


            modelRecord = await _cacheService.AddCacheAsync(modelRecord, _clientId, UseCache);

            //Check model record editable status as per the preference value
            modelRecord.CheckIsEditable(_modificationInDays);
            return await Task.Run(() => SuccessResponse<VendorModel, VendorDtoModel>(modelRecord.Id, modelRecord));
        }

        public async Task<ResponseDto<VendorDtoModel>> UpdateAsync(string id, VendorDtoModel dto,
            bool UseCache = true)
        {

            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for updating record!".MarkInformation());

            ResponseDto<VendorDtoModel> responseDto = new(_httpContextAccessor);
            VendorModel? modelRecord = new VendorModel();

            //check record availability by name
            if (await FindExistingRecordAsync(dto.ContactNo, dto.Id, _modificationInDays, _clientId, UseCache))
            {
                return DuplicateEntryResponse<VendorDtoModel>("Contact No", dto.ContactNo);
            }

            //check record availability by id
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), $"Checking records for id {id}");
            modelRecord = await _cacheService.GetCacheAsync(id, _modificationInDays, _clientId, UseCache);

            //When no record available.
            if (modelRecord == null || modelRecord.Id != dto.Id)
            {

                return FailureResponse<VendorDtoModel>(CommonMessages.RecordNotFound);
            }
            //when no permission for updating record
            else if (modelRecord.IsEditable == false)
            {
                return FailureResponse<VendorDtoModel>(CommonMessages.NotPermitted);
            }

            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), $"Record found updating!");

            modelRecord = _mapper.Map<VendorModel>(dto);

            modelRecord.Save("", _userId);
            modelRecord.Update("", _userId);

            modelRecord = await _cacheService.UpdateCacheAsync(modelRecord, _clientId, UseCache);
            if (string.IsNullOrWhiteSpace(modelRecord.Id))
            {
                return FailureResponse<VendorDtoModel>(CommonMessages.OperationFailed);
            }

            //Check model record editable status as per the preference value
            modelRecord.CheckIsEditable(_modificationInDays);

            return await Task.Run(() => SuccessResponse<VendorModel, VendorDtoModel>(modelRecord.Id, modelRecord));
        }

        public async Task<ResponseDto<VendorDtoModel>> DeleteAsync(string id, bool UseCache = true)
        {

            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for deleting record!".MarkInformation());

            ResponseDto<VendorDtoModel> responseDto = new(_httpContextAccessor);

            //check record availability by id
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), $"Checking records for id {id}");
            var modelRecord = await _cacheService.GetCacheAsync(id, _modificationInDays, _clientId, UseCache);
            if (modelRecord == null)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. Record not available!!!");
                return FailureResponse<VendorDtoModel>(CommonMessages.RecordNotFound);
            }
            else if (!modelRecord.IsEditable)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. Permission not granted!!!");
                return FailureResponse<VendorDtoModel>(CommonMessages.NotPermitted);
            }

            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), $"Record found for marking it deleted!");

            //check if record reference in other table
            //right now no reference table we have checked.
            var recordUsed = await _cacheService.IsRecordInUseAsync(id);

            if (recordUsed)
            {
                return FailureResponse<VendorDtoModel>(CommonMessages.RecordInUsed);

            }

            //here we are using model behavior
            modelRecord.Delete(_userId);

            modelRecord.RecordStatus = await _cacheService.DeleteCacheAsync(modelRecord.Id, modelRecord, _modificationInDays, UseCache);
            if (modelRecord.RecordStatus != GenericFunction.Enums.EnumRecordStatus.Deleted)
            {
                return FailureResponse<VendorDtoModel>(CommonMessages.OperationFailed);
            }


            return await Task.Run(() => SuccessResponse<VendorModel, VendorDtoModel>(modelRecord.Id, modelRecord));

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
            List<VendorModel>? records = await _cacheService.GetAllCacheAsync(_modificationInDays, _clientId, useCache);
            //if no records inserted in db or cache
            if (records == null || records?.Count == 0)
                return false;

            //id is null while adding new record, don't change the condition
            if (string.IsNullOrWhiteSpace(id))
            {
                //New Record

                return await Task.Run(() => records.Any(m => m.ContactNo == fieldName
                                            && m.RecordStatus == EnumRecordStatus.Active));
            }
            else
            {
                //when record id is available it mean we are updating record, so checking 
                //condition for existing record.
                return await Task.Run(() => records.Any(m => m.ContactNo.ToUpper() == fieldName.ToUpper()
                                                             && m.RecordStatus == EnumRecordStatus.Active
                                                             && m.Id != id));
            }
        }
    }



}