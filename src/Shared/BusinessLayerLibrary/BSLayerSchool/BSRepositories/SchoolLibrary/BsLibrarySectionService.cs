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
namespace BSLayerSchool.BSRepositories.SchoolLibrary
{
    public sealed class BsLibrarySectionService : BaseBusinessLayer, IBsLibrarySectionContract
    {
        private readonly IDataLayerLibrarySectionContract _dlService;

        public BsLibrarySectionService(IDataLayerLibrarySectionContract dlService, ICacheContract _cache, IHttpContextAccessor httpContextAccessor, IMapper iMapper, ITrace trace)
        : base(iMapper, httpContextAccessor, trace, _cache)
        {
            _dlService = dlService;
            _expirationTime = _applicationSettings.ModuleCacheSettings.LibraryModule.GetKeyLifeForCacheStorage();
        }

        public async Task<ResponseDto<LibrarySectionDtoModel>> Get(string id, string roomId, bool UseCache = true)
        {


            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started".MarkInformation());



            ResponseDto<LibrarySectionDtoModel> responseDto = new(_httpContextAccessor);


            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), $"Checking records for id {id}");

            //First check cache entry
            List<LibrarySectionModel>? modelRecords = new List<LibrarySectionModel>();
            LibrarySectionModel? modelRecord = new();
            //Reading records from cache
            if (UseCache)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Reading from Cache Server!".MarkInformation());
                modelRecords = await _cacheData.ReadFromCacheAsync<LibrarySectionModel>("Index", _modificationInDays);
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
                modelRecord = await _dlService.GetAsync(id, roomId);
                if (modelRecord != null)
                {
                    modelRecords = new List<LibrarySectionModel>();
                    modelRecords.Append(modelRecord);
                    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, $"Record found from database server!".MarkProcess());
                    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Record saving in cache server!".MarkProcess());
                    await CacheService.SetDataAsync(_cacheData.GenerateKeyForCache<LibrarySectionModel>("Index", _clientId), modelRecords, _expirationTime);
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


            //watch.Stop();
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
                Result = _mapper.Map<LibrarySectionDtoModel>(modelRecord)
            };
            return await Task.Run(() => responseDto);
        }

        public async Task<ResponseDto<List<LibrarySectionDtoModel>>> GetAll(string roomId, int pageNo, int pageSize,
            bool UseCache = true)
        {


            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started".MarkInformation());



            //First check cache entry
            List<LibrarySectionModel>? modelRecords = new List<LibrarySectionModel>();

            //Reading records from cache
            if (UseCache)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Reading from Cache Server!".MarkInformation());
                modelRecords = await _cacheData.ReadFromCacheAsync<LibrarySectionModel>("Index", _modificationInDays);
                modelRecords = modelRecords?.Where(m => m.RoomId == roomId).ToList();
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), $"Reading completed from Cache Server!".MarkInformation());
            }


            //If records are not available in cache then check db.
            if (modelRecords == null || modelRecords?.Count() == 0)
            {

                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Reading from Database Server!".MarkInformation());
                modelRecords = await _dlService.GetAllAsync(roomId);
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Reading completed from Database Server!".MarkInformation());
                if (modelRecords != null && modelRecords.Count() > 0)
                {
                    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, $"{modelRecords?.Count()} records found from database server!".MarkInformation());
                    await CacheService.SetDataAsync(_cacheData.GenerateKeyForCache<LibrarySectionModel>("Index", _clientId), modelRecords, _expirationTime);
                    modelRecords = modelRecords?.Where(m => m.RoomId == roomId).ToList();
                    _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, "Records saved in cache server!".MarkInformation());
                }
            }

            ResponseDto<List<LibrarySectionDtoModel>>? responseDto = new(_httpContextAccessor);

            if (modelRecords == null || modelRecords?.Count() == 0)
            {
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. No data available in database and cache server!");
                //watch.Stop();
                responseDto.Log = _trace.GetTraceLogs("");
                responseDto.Message = RecordNotFound;
                responseDto.TimeConsumption = 0;
                responseDto.Result = _mapper.Map<List<LibrarySectionDtoModel>>(modelRecords);
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
                CurrentPageNo = fetchRecords.PageNumber, // pageNo == 0 ? 1 : pageNo,
                Pages = fetchRecords.PageCount, //(modelRecords.Count + pageSize - 1) / pageSize,
                Message = OperationSuccessful,
                MessageType = MessageType.Information,
                Status = Status.Success,
                StatusCode = StatusCodes.Status200OK,
                Result = _mapper.Map<List<LibrarySectionDtoModel>>(modelRecords)
            };
            return await Task.Run(() => responseDto);
        }




        public async Task<ResponseDto<LibrarySectionDtoModel>> AddAsync(LibrarySectionDtoModel dto, bool UseCache = true)
        {


            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for adding new record!".MarkInformation());


            ResponseDto<LibrarySectionDtoModel> responseDto = new(_httpContextAccessor);

            //find record from repository
            //check record availability by name
            if (await _dlService.FindExistingRecordAsync(dto.Name, dto.Id, dto.RoomId))
            {
                return DuplicateEntryResponse<LibrarySectionDtoModel>("Section Name", dto.Name);
            }
            //Do all business operation here
            var modelRecord = new LibrarySectionModel();
            modelRecord = _mapper.Map<LibrarySectionModel>(dto);
            modelRecord.Save(_userId);

            //Cache response from repository and sent back
            modelRecord = await _dlService.AddAsync(modelRecord);

            //Check model record editable status as per the preference value
            modelRecord.CheckIsEditable(_modificationInDays);

            //Add record to cache
            var cacheResult = await UpdateCacheEntry<LibrarySectionModel>(modelRecord, isNew: true);
            if (cacheResult.status == false)
            {
                await GetAll(modelRecord.RoomId, 1, _resultPerPage, true);
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
                Result = _mapper.Map<LibrarySectionDtoModel>(modelRecord)
            };
            return await Task.Run(() => responseDto);
        }

        public async Task<ResponseDto<LibrarySectionDtoModel>> UpdateAsync(string id, LibrarySectionDtoModel dto,
            bool UseCache = true)
        {



            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for updating record!".MarkInformation());

            ResponseDto<LibrarySectionDtoModel> responseDto = new(_httpContextAccessor);

            //check record availability by name
            if (await _dlService.FindExistingRecordAsync(dto.Name, dto.Id, dto.RoomId))
            {
                return DuplicateEntryResponse<LibrarySectionDtoModel>("Name", dto.Name);
            }

            //check record availability by id
            LibrarySectionModel? modelRecord = await _dlService.GetAsync(id, dto.RoomId);

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

            modelRecord = _mapper.Map<LibrarySectionModel>(dto);
            modelRecord.Save(_userId);
            modelRecord.Update();

            await _dlService.UpdateAsync(modelRecord);

            //Check model record editable status as per the preference value
            modelRecord.CheckIsEditable(_modificationInDays);

            //Updating cache
            var cacheResult = await UpdateCacheEntry<LibrarySectionModel>(modelRecord, isNew: false);
            if (cacheResult.status == false)
            {
                await GetAll(modelRecord.RoomId, 1, _resultPerPage, true);
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
                Result = _mapper.Map<LibrarySectionDtoModel>(modelRecord)
            };
            return await Task.Run(() => responseDto);
        }

        public async Task<ResponseDto<LibrarySectionDtoModel>> DeleteAsync(string id, string roomId, bool UseCache = true)
        {


            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for deleting record!".MarkInformation());



            ResponseDto<LibrarySectionDtoModel> responseDto = new(_httpContextAccessor);

            //check record availability by id
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), $"Checking records for id {id}");
            var modelRecord = await _dlService.GetAsync(id, roomId);
            if (modelRecord == null)
            {
                //watch.Stop();
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. Record not available!!!");
                responseDto.Log = _trace.GetTraceLogs("");
                responseDto.TimeConsumption = 0;
                responseDto.Message = RecordNotFound;
                responseDto.Result = null;
                return responseDto;
            }
            else if (!modelRecord.IsEditable)
            {
                //watch.Stop();
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. Permission not granted!!!");
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
                //watch.Stop();
                _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. Record reference is already in used!!!");
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
                DeleteCacheEntry<LibrarySectionModel>(id);
            }

            //Return record.
            //watch.Stop();
            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), "Process end. Record deleted. All success!!!");

            responseDto = new(_httpContextAccessor)
            {
                Log = _trace.GetTraceLogs(""),
                Id = modelRecord.Id,
                DateTime = DateTime.Now,
                Message = RecordDeleted,
                MessageType = MessageType.Information,
                Status = Status.Success,
                RecordCount = 1,
                StatusCode = StatusCodes.Status200OK,
                TimeConsumption = 0,
                Result = _mapper.Map<LibrarySectionDtoModel>(modelRecord)
            };
            return await Task.Run(() => responseDto);

        }
    }
}

