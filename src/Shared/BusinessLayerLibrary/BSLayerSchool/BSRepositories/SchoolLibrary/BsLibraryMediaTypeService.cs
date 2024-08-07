using AutoMapper;
using BSLayerSchool.BaseClass;
using BSLayerSchool.BSInterfaces.SchoolLibraryContracts;
using DataBaseServices.Core.Contracts.SchoolLibraryContracts;
using DataCacheLayer.CacheRepositories;
using DataCacheLayer.CacheRepositories.Interfaces;
using GenericFunction;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using ModelTemplates.DtoModels.SchoolLibrary;
using ModelTemplates.Persistence.Models.School.Library;
using PagedList;
using static GenericFunction.CommonMessages;
namespace BSLayerSchool.BSRepositories.CommonServices
{
    public sealed class BsLibraryMediaTypeService : BaseBusinessLayer, IBsLibraryMediaTypeContract
    {
        private readonly IDataLayerLibraryMediaTypeContract _dlService;

        public BsLibraryMediaTypeService(IDataLayerLibraryMediaTypeContract dlService, ICacheContract _cache, IHttpContextAccessor httpContextAccessor, IMapper iMapper, ITrace trace)
       : base(iMapper, httpContextAccessor, trace, _cache)
        {
            _dlService = dlService;
            _expirationTime = _applicationSettings.ModuleCacheSettings.LibraryModule.GetKeyLifeForCacheStorage();
        }

        /// <summary>
        /// Get record
        /// </summary>
        /// <param name="id">record id</param>
        /// <param name="UseCache">true for use Redis cache, default is true</param>
        /// <returns>ResponseDto object</returns>

        public async Task<ResponseDto<LibraryMediaTypeDtoModel>> Get(string id, bool UseCache = true)
        {


            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started".MarkInformation());



            ResponseDto<LibraryMediaTypeDtoModel> responseDto = new(_httpContextAccessor);
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), $"Checking records for id {id}");

            //First check cache entry
            List<LibraryMediaTypeModel>? modelRecords = new List<LibraryMediaTypeModel>();
            LibraryMediaTypeModel? modelRecord = new();
            //Reading records from cache
            if (UseCache)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Reading from Cache Server!".MarkInformation());
                modelRecords = await _cacheData.ReadFromCacheAsync<LibraryMediaTypeModel>("Index", _modificationInDays);
                if (modelRecords != null)
                {
                    modelRecord = modelRecords.FirstOrDefault(m => m.Id == id);
                }
                else
                {
                    modelRecord = null;
                    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, $"Record not found from Cache Server!".MarkInformation());
                }
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), $"Reading completed from Cache Server!".MarkInformation());
            }

            //when record is not available in cache.
            if (modelRecord == null)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Reading from Database Server!".MarkInformation());
                modelRecord = await _dlService.GetAsync(id);
                if (modelRecord != null)
                {
                    modelRecords = new List<LibraryMediaTypeModel>();
                    modelRecords.Append(modelRecord);
                    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, $"Record found from database server!".MarkProcess());
                    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Record saving in cache server!".MarkProcess());
                    await CacheService.SetDataAsync(_cacheData.GenerateKeyForCache<LibraryMediaTypeModel>("Index", _clientId), modelRecords, _expirationTime);
                    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Records saved in cache server!".MarkProcess());
                }
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Reading from Database Server completed!".MarkInformation());
            }

            //if record not found
            if (modelRecord == null)
            {
                //watch.Stop();
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. No data available in database!");
                responseDto.Message = RecordNotFound;
                responseDto.Result = null;
                responseDto.Log = _trace.GetTraceLogs("");
                return responseDto;
            }

            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. Record found!");

            //Send response to controller class
            responseDto = new(_httpContextAccessor)
            {
                Id = id,
                Log = _trace.GetTraceLogs(""),
                DateTime = DateTime.Now,
                Message = OperationSuccessful,
                MessageType = MessageType.Information,
                Status = Status.Success,
                RecordCount = 1,
                StatusCode = StatusCodes.Status200OK,
                TimeConsumption = 0,
                Result = _mapper.Map<LibraryMediaTypeDtoModel>(modelRecord)
            };
            return await Task.Run(() => responseDto);
        }

        public async Task<ResponseDto<List<LibraryMediaTypeDtoModel>>> GetAll(int pageNo, int pageSize,
            bool UseCache = true)
        {


            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started".MarkInformation());



            //First check cache entry
            List<LibraryMediaTypeModel>? modelRecords = new List<LibraryMediaTypeModel>();

            //Reading records from cache
            if (UseCache)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Reading from Cache Server!".MarkInformation());
                modelRecords = await _cacheData.ReadFromCacheAsync<LibraryMediaTypeModel>("Index", _modificationInDays);
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), $"Reading completed from Cache Server!".MarkInformation());
            }

            //If records are not available in cache then check db.
            if (modelRecords == null || modelRecords?.Count() == 0)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Reading from Database Server!".MarkInformation());
                modelRecords = await _dlService.GetAllAsync();
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Reading completed from Database Server!".MarkInformation());
                if (modelRecords != null && modelRecords.Count() > 0)
                {
                    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, $"{modelRecords?.Count()} records found from database server!".MarkInformation());
                    await CacheService.SetDataAsync(_cacheData.GenerateKeyForCache<LibraryMediaTypeModel>("Index", _clientId), modelRecords, _expirationTime);
                    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, "Records saved in cache server!".MarkInformation());
                }
            }

            ResponseDto<List<LibraryMediaTypeDtoModel>>? responseDto = new(_httpContextAccessor);

            if (modelRecords == null || modelRecords?.Count() == 0)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. No data available in database and cache server!");
                //watch.Stop();
                responseDto.Log = _trace.GetTraceLogs("");
                responseDto.Message = RecordNotFound;
                responseDto.TimeConsumption = 0;
                responseDto.Result = _mapper.Map<List<LibraryMediaTypeDtoModel>>(modelRecords);
                responseDto.CurrentPageNo = pageNo;
                responseDto.Pages = 0;
                responseDto.Status = Status.Success;
                responseDto.RecordCount = 0;
                responseDto.StatusCode = StatusCodes.Status200OK;
                return responseDto;
            }


            //When records available, add pagination on that records.
            pageSize = pageSize == 0 ? _resultPerPage : pageSize;

            var fetchRecords =
               modelRecords.ToPagedList(pageNo == 0 ? 1 : pageNo, pageSize == 0 ? _resultPerPage : pageSize);

            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. All success!!!");
            //watch.Stop();
            responseDto = new(_httpContextAccessor)
            {
                Log = _trace.GetTraceLogs(""),
                DateTime = DateTime.Now,
                TimeConsumption = 0,
                RecordCount = fetchRecords?.Count(),
                CurrentPageNo = fetchRecords != null ? fetchRecords.PageNumber : 0, // pageNo == 0 ? 1 : pageNo,
                Pages = fetchRecords != null ? fetchRecords.PageCount : 0, //(modelRecords.Count + pageSize - 1) / pageSize,
                Message = OperationSuccessful,
                MessageType = MessageType.Information,
                Status = Status.Success,
                StatusCode = StatusCodes.Status200OK,
                Result = _mapper.Map<List<LibraryMediaTypeDtoModel>>(modelRecords),

            };
            return await Task.Run(() => responseDto);
        }


        public async Task<ResponseDto<LibraryMediaTypeDtoModel>> AddAsync(LibraryMediaTypeDtoModel dto, bool UseCache = true)
        {


            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for adding new record!".MarkInformation());



            //here useCache is pending for implementations

            ResponseDto<LibraryMediaTypeDtoModel> responseDto = new(_httpContextAccessor);

            //find record from repository
            //check record availability by name
            if (await _dlService.FindExistingRecordAsync(dto.Name, dto.Id))
            {
                return DuplicateEntryResponse<LibraryMediaTypeDtoModel>("Name", dto.Name);
            }
            //Do all business operation here
            var modelRecord = new LibraryMediaTypeModel();
            modelRecord = _mapper.Map<LibraryMediaTypeModel>(dto);
            modelRecord.Save(_userId);

            //Cache response from repository and sent back
            modelRecord = await _dlService.AddAsync(modelRecord);

            //Check model record editable status as per the preference value
            modelRecord.CheckIsEditable(_modificationInDays);

            //Add record to cache
            var cacheResult = await UpdateCacheEntry<LibraryMediaTypeModel>(modelRecord, isNew: true);
            if (cacheResult.status == false)
            {
                await GetAll(1, _resultPerPage, true);
            }

            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. All success!!!");


            responseDto = new(_httpContextAccessor)
            {
                Log = _trace.GetTraceLogs(""),
                Id = modelRecord.Id,
                DateTime = DateTime.Now,
                Message = OperationSuccessful,
                MessageType = MessageType.Information,
                Status = Status.Success,
                RecordCount = 1,
                StatusCode = StatusCodes.Status200OK,
                TimeConsumption = 0,
                Result = _mapper.Map<LibraryMediaTypeDtoModel>(modelRecord)
            };
            return await Task.Run(() => responseDto);
        }

        public async Task<ResponseDto<LibraryMediaTypeDtoModel>> UpdateAsync(string id, LibraryMediaTypeDtoModel dto,
            bool UseCache = true)
        {



            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for updating record!".MarkInformation());



            ResponseDto<LibraryMediaTypeDtoModel> responseDto = new(_httpContextAccessor);

            //check record availability by name
            if (await _dlService.FindExistingRecordAsync(dto.Name, dto.Id))
            {
                return DuplicateEntryResponse<LibraryMediaTypeDtoModel>("Media type", dto.Name);
            }

            //check record availability by id
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), $"Checking records for id {id}");
            LibraryMediaTypeModel? modelRecord = await _dlService.GetAsync(id);

            //When no record available.
            if (modelRecord == null)
            {
                //watch.Stop();
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. Record not available!!!");
                responseDto.Log = _trace.GetTraceLogs("");
                responseDto.TimeConsumption = 0;
                responseDto.Message = RecordNotFound;
                responseDto.Result = dto;
                return responseDto;
            }
            //when no permission for updating record
            else if (modelRecord.IsEditable == false)
            {
                //watch.Stop();
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. Permission not granted!!!");
                responseDto.Log = _trace.GetTraceLogs("");
                responseDto.Message = NotPermitted;
                responseDto.Result = dto;
                return responseDto;
            }
            if (modelRecord.Id != dto.Id)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. Record not matched with id!!!");
                responseDto.TimeConsumption = 0;
                responseDto.Message = RecordNotFound;
                responseDto.Result = dto;
                responseDto.Log = _trace.GetTraceLogs("");
                return responseDto;

            }

            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), $"Record found updating!");

            modelRecord = _mapper.Map<LibraryMediaTypeModel>(dto);
            modelRecord.Save(_userId);
            modelRecord.Update();

            await _dlService.UpdateAsync(modelRecord);

            //Check model record editable status as per the preference value
            modelRecord.CheckIsEditable(_modificationInDays);


            //Updating cache
            var cacheResult = await UpdateCacheEntry<LibraryMediaTypeModel>(modelRecord, isNew: false);
            if (cacheResult.status == false)
            {
                await GetAll(1, _resultPerPage, true);
            }

            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. Record updated! All success!!!");
            responseDto = new(_httpContextAccessor)
            {
                Log = _trace.GetTraceLogs(""),
                Id = modelRecord.Id,
                DateTime = DateTime.Now,
                Message = OperationSuccessful,
                MessageType = MessageType.Information,
                Status = Status.Success,
                RecordCount = 1,
                StatusCode = StatusCodes.Status200OK,
                TimeConsumption = 0,
                Result = _mapper.Map<LibraryMediaTypeDtoModel>(modelRecord)
            };
            return await Task.Run(() => responseDto);
        }

        public async Task<ResponseDto<LibraryMediaTypeDtoModel>> DeleteAsync(string id, bool UseCache = true)
        {



            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for deleting record!".MarkInformation());



            ResponseDto<LibraryMediaTypeDtoModel> responseDto = new(_httpContextAccessor);

            //check record availability by id
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), $"Checking records for id {id}");
            var modelRecord = await _dlService.GetAsync(id);
            if (modelRecord == null)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. Record not available!!!");
                //watch.Stop();
                responseDto.Log = _trace.GetTraceLogs("");
                responseDto.TimeConsumption = 0;
                responseDto.Message = RecordNotFound;
                responseDto.Result = null;
                return responseDto;
            }
            else if (!modelRecord.IsEditable)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. Permission not granted!!!");
                //watch.Stop();
                responseDto.Log = _trace.GetTraceLogs("");
                responseDto.TimeConsumption = 0;
                responseDto.Message = NotPermitted;
                responseDto.Result = null;
                return responseDto;

            }

            //check if record is used in other table
            var recordUsed = await _dlService.IsRecordInUseAsync(id);//.AnyAsync(m => m.LibraryId == id);

            if (recordUsed)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. Record reference is already in used!!!");
                //watch.Stop();
                responseDto.Log = _trace.GetTraceLogs("");
                responseDto.TimeConsumption = 0;
                responseDto.Message = RecordInUsed;
                responseDto.Result = null;
                return responseDto;

            }
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), $"Record found for marking it deleted!");
            modelRecord.Delete(_userId);

            await _dlService.DeleteAsync();


            //Remove record from cache too
            if (UseCache)
            {
                DeleteCacheEntry<LibraryMediaTypeModel>(id);
            }

            //Return record.
            //watch.Stop();
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. Record deleted. All success!!!");
            responseDto = new(_httpContextAccessor)
            {
                Id = modelRecord.Id,
                DateTime = DateTime.Now,
                Message = RecordDeleted,
                MessageType = MessageType.Information,
                Status = Status.Success,
                RecordCount = 1,
                StatusCode = StatusCodes.Status200OK,
                TimeConsumption = 0,
                Result = _mapper.Map<LibraryMediaTypeDtoModel>(modelRecord),
                Log = _trace.GetTraceLogs(""),
            };
            return await Task.Run(() => responseDto);

        }
    }
}

