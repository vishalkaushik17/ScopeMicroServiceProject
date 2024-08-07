using AutoMapper;
using BSLayerSchool.BaseClass;
using BSLayerSchool.BSInterfaces.EmployeeContracts;
using DataCacheLayer.CacheRepositories.Interfaces;
using GenericFunction;
using GenericFunction.Enums;
using GenericFunction.ResultObject;
using MCEmployeeLayer.Interface;
using Microsoft.AspNetCore.Http;
using ModelTemplates.DtoModels.Employee;
using ModelTemplates.Persistence.Models.School.Employee;
using static GenericFunction.CommonMessages;
namespace BSLayerSchool.BSRepositories.InventoryManagement
{
    public class BsEmployeeStudentParentService : BaseBusinessLayer, IBsEmployeeStudentParentContract
    {
        private readonly IMCEmployeeStudentParentContract _cacheService;

        public BsEmployeeStudentParentService(IMCEmployeeStudentParentContract cacheService, ICacheContract _cache, IHttpContextAccessor httpContextAccessor, IMapper iMapper, ITrace trace)
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

        public async Task<ResponseDto<EmployeeStudentParentDtoModel>> Get(string id, bool UseCache = true)
        {

            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started".MarkInformation());

            EmployeeStudentParentModel? modelRecord = await _cacheService.GetCacheAsync(id, _modificationInDays, _clientId, UseCache);

            if (modelRecord == null)
            {
                return FailureResponse<EmployeeStudentParentDtoModel>(CommonMessages.RecordNotFound);
            }


            //Send response to controller class
            return await Task.Run(() => SuccessResponse<EmployeeStudentParentModel, EmployeeStudentParentDtoModel>(id, modelRecord));
        }

        public async Task<ResponseDto<List<EmployeeStudentParentDtoModel>>> GetAll(int pageNo, int pageSize,
            bool UseCache = true)
        {
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started".MarkInformation());

            //First check cache entry
            List<EmployeeStudentParentModel>? modelRecords = new List<EmployeeStudentParentModel>();

            modelRecords = await _cacheService.GetAllCacheAsync(_modificationInDays, _clientId, UseCache);


            if (modelRecords == null || modelRecords?.Count() == 0)
            {
                return FailureResponse<List<EmployeeStudentParentDtoModel>>(CommonMessages.RecordNotFound);
            }

            return await Task.Run(() => SuccessResponse<EmployeeStudentParentModel, List<EmployeeStudentParentDtoModel>>(modelRecords));
        }

        public async Task<ResponseDto<EmployeeStudentParentDtoModel>> AddAsync(EmployeeStudentParentDtoModel dto, bool UseCache = true)
        {
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for adding new record!".MarkInformation());

            //find record from repository
            //check record availability by name
            if (await FindExistingRecordAsync(dto.UIDNo, dto.Id, _modificationInDays, _clientId, UseCache))
            {
                return DuplicateEntryResponse<EmployeeStudentParentDtoModel>("Uid No", dto.UIDNo);
            }

            //Do all business operation here
            var modelRecord = new EmployeeStudentParentModel();

            modelRecord = _mapper.Map<EmployeeStudentParentModel>(dto);

            modelRecord = await _cacheService.AddCacheAsync(modelRecord, _clientId, UseCache);
            if (string.IsNullOrWhiteSpace(modelRecord.Id))
            {
                return FailureResponse<EmployeeStudentParentDtoModel>(CommonMessages.OperationFailed);
            }
            modelRecord.CheckIsEditable(_modificationInDays);

            return await Task.Run(() => SuccessResponse<EmployeeStudentParentModel, EmployeeStudentParentDtoModel>(modelRecord.Id, modelRecord));
        }

        public async Task<ResponseDto<EmployeeStudentParentDtoModel>> UpdateAsync(string id, EmployeeStudentParentDtoModel dto,
            bool UseCache = true)
        {
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for updating record!".MarkInformation());

            //check record availability by name
            if (await FindExistingRecordAsync(dto.UIDNo, dto.Id, _modificationInDays, _clientId, UseCache))
            {
                return DuplicateEntryResponse<EmployeeStudentParentDtoModel>("Uid No", dto.UIDNo);
            }

            //check record availability by id
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), $"Checking records for id {id}");
            EmployeeStudentParentModel? modelRecord = await _cacheService.GetCacheAsync(id, _modificationInDays, _clientId, UseCache);

            //When no record available.
            if (modelRecord == null || modelRecord.Id != dto.Id)
            {
                return FailureResponse<EmployeeStudentParentDtoModel>(CommonMessages.RecordNotFound);
            }
            //when no permission for updating record
            else if (modelRecord.IsEditable == false)
            {
                //watch.Stop();
                return FailureResponse<EmployeeStudentParentDtoModel>(CommonMessages.NotPermitted);
            }

            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), $"Record found updating!");
            modelRecord = _mapper.Map<EmployeeStudentParentModel>(dto);

            await _cacheService.UpdateCacheAsync(modelRecord, _clientId, UseCache);
            modelRecord.CheckIsEditable(_modificationInDays);
            return await Task.Run(() => SuccessResponse<EmployeeStudentParentModel, EmployeeStudentParentDtoModel>(modelRecord.Id, modelRecord));


        }

        public async Task<ResponseDto<EmployeeStudentParentDtoModel>> DeleteAsync(string id, bool UseCache = true)
        {
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for deleting record!".MarkInformation());

            //check record availability by id
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), $"Checking records for id {id}");
            EmployeeStudentParentModel modelRecord = await _cacheService.GetCacheAsync(id, _modificationInDays, _clientId, UseCache);

            if (modelRecord == null)
            {
                return FailureResponse<EmployeeStudentParentDtoModel>(CommonMessages.RecordNotFound);
            }
            else if (!modelRecord.IsEditable)
            {
                FailureResponse<EmployeeStudentParentDtoModel>(CommonMessages.NotPermitted);
            }
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), $"Record found for marking it deleted!");

            //check if record is used in other table
            bool recordUsed = await _cacheService.IsRecordInUseAsync(id);//.AnyAsync(m => m.LibraryId == id);

            if (recordUsed)
            {
                return FailureResponse<EmployeeStudentParentDtoModel>(CommonMessages.RecordInUsed);
            }


            modelRecord.RecordStatus = await _cacheService.DeleteCacheAsync(id, modelRecord, _modificationInDays, UseCache);


            if (modelRecord.RecordStatus != GenericFunction.Enums.EnumRecordStatus.Deleted)
            {
                return FailureResponse<EmployeeStudentParentDtoModel>(CommonMessages.OperationFailed);
            }

            return await Task.Run(() => SuccessResponse<EmployeeStudentParentModel, EmployeeStudentParentDtoModel>(modelRecord.Id, modelRecord));

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
            List<EmployeeStudentParentModel>? records = await _cacheService.GetAllCacheAsync(_modificationInDays, _clientId, useCache);
            //if no records inserted in db or cache
            if (records == null || records?.Count == 0)
                return false;

            //id is null while adding new record, don't change the condition
            if (string.IsNullOrWhiteSpace(id))
            {
                //New Record

                return await Task.Run(() => records.Any(m => m.UIDNo.ToUpper() == fieldName
                                                             && m.RecordStatus == EnumRecordStatus.Active));
            }
            else
            {
                //when record id is available it mean we are updating record, so checking 
                //condition for existing record.
                return await Task.Run(() => records.Any(m => m.UIDNo.ToUpper() == fieldName
                                                             && m.RecordStatus == EnumRecordStatus.Active
                                                             && m.Id != id));
            }
        }


    }
}

