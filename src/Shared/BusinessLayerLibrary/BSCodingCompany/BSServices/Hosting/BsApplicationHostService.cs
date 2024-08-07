using AutoMapper;
using BSCodingCompany.BaseClass;
using BSLayerSchool.BSInterfaces.SchoolLibraryContracts;
using DataBaseServices.Core.Contracts.CommonServices;
using DataCacheLayer.CacheRepositories;
using DataCacheLayer.CacheRepositories.Interfaces;
using GenericFunction;
using GenericFunction.ResultObject;
using GenericFunction.ServiceObjects.EncryptionService;
using Microsoft.AspNetCore.Http;
using ModelTemplates.DtoModels.AppConfig;
using ModelTemplates.EntityModels.AppConfig;
using ModelTemplates.Persistence.Models.School.CommonModels;
using PagedList;
using static GenericFunction.CommonMessages;
namespace BSCodingCompany.BSServices.Hosting;
public sealed class BsApplicationHostService : BaseBusinessLayer, IBsApplicationHostContract
{
    private readonly IDataLayerApplicationHostContract _dlService;
    private readonly ICacheContract _cache;
    private readonly string _hashString = string.Empty;

    public BsApplicationHostService(IDataLayerApplicationHostContract dbService, ICacheContract cache, IHttpContextAccessor httpContextAccessor, IMapper mapper, ITrace trace) :
        base(mapper, httpContextAccessor, trace, cache)
    {
        _dlService = dbService;
        _cache = cache;

        _hashString = ExtensionMethods.GeneratePassword();

    }

    /// <summary>
    /// Add record where database get hosted for client.
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="useCache"></param>
    /// <returns></returns>
    public async Task<ResponseDto<ApplicationHostMasterDtoModel>> AddAsync(ApplicationHostMasterDtoModel dto, bool useCache = true)
    {


        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for adding new record!".MarkInformation());

        ResponseDto<ApplicationHostMasterDtoModel> responseDto = new(_httpContextAccessor);

        //find record from repository
        //check record availability by name


        try
        {
            dto.ConnectionString = IEncryptionService.Encrypt(dto.ConnectionString, _applicationSettings.CacheServer.HashKey);
            dto.UserName = IEncryptionService.Encrypt(dto.UserName, _applicationSettings.CacheServer.HashKey);
            dto.Domain = IEncryptionService.Encrypt(dto.Domain, _applicationSettings.CacheServer.HashKey);
            dto.HashString = _applicationSettings.CacheServer.HashKey;

            if (await _dlService.FindExistingRecordAsync(dto.Domain, dto.Id))
            {
                DuplicateEntryCheck(dto, responseDto, "Connection String", dto.ConnectionString);
                return responseDto;
            }
            var modelRecord = _mapper.Map<ApplicationHostMasterModel>(dto);
            modelRecord.Save(_userId);
            //need to work here
            modelRecord = await _dlService.AddAsync(modelRecord);

            _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, $"Record added!".MarkInformation());
            modelRecord = await _dlService.GetSpecificRecordAsync(modelRecord.ConnectionString, modelRecord.CreatedOn);

            //updating newly created record in cache.
            var cacheResult = await UpdateCacheEntry<ApplicationHostMasterModel>(modelRecord, isNew: true);
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
                Result = _mapper.Map<ApplicationHostMasterDtoModel>(modelRecord)
            };
            return await Task.Run(() => responseDto);


        }
        catch (Exception ex)
        {
            ex.SendExceptionMailAsync().GetAwaiter().GetResult();
            responseDto.Message += ex.Message;
        }
        return responseDto;

    }

    public async Task<ResponseDto<ApplicationHostMasterDtoModel>> Get(string id, bool useCache = true)
    {
        ResponseDto<ApplicationHostMasterDtoModel> responseDto = new(_httpContextAccessor);
        try
        {
            var record = await _dlService.GetAsync(id);
            if (record == null)
            {
                return responseDto;
            }

            responseDto.Result = _mapper.Map<ApplicationHostMasterDtoModel>(record);
            responseDto.UserName = IEncryptionService.Decrypt(record.UserName, record.HashString);


            responseDto.Message = CommonMessages.OperationSuccessful;
            responseDto.MessageType = MessageType.Information;
            responseDto.Status = Status.Success;
            responseDto.RecordCount = 1;
            responseDto.StatusCode = StatusCodes.Status200OK;
            responseDto.Id = record.Id;

        }
        catch (Exception ex)
        {
            ex.SendExceptionMailAsync().GetAwaiter().GetResult();
            responseDto.Message += ex.Message;
        }

        return responseDto;
    }
    public async Task<ResponseDto<List<ApplicationHostMasterDtoModel>>> GetAll(int pageNo, int pageSize,
        bool useCache = true)
    {

        //var watch = System.Diagnostics.Stopwatch.Startnew(_httpContextAccessor);


        //First check cache entry
        List<ApplicationHostMasterModel>? modelRecords = new();
        //Reading records from cache
        if (UseCache)
        {
            modelRecords = await _cacheData.ReadFromCacheAsync<ApplicationHostMasterModel>("Index", _modificationInDays);
        }


        //If records are not available in cache then check db.
        if (modelRecords == null || modelRecords?.Count() == 0)
        {
            modelRecords = await _dlService.GetAllAsync();
            if (modelRecords != null && modelRecords.Count() > 0)
                await CacheService.SetDataAsync(_cacheData.GenerateKeyForCache<CurrencyModel>("Index", _clientId), modelRecords, _expirationTime);

        }

        ResponseDto<List<ApplicationHostMasterDtoModel>>? responseDto = new(_httpContextAccessor);

        if (modelRecords == null || modelRecords?.Count() == 0)
        {
            //watch.Stop();
            responseDto.Message = RecordNotFound;
            responseDto.TimeConsumption = 0;
            responseDto.Result = _mapper.Map<List<ApplicationHostMasterDtoModel>>(modelRecords);
            responseDto.CurrentPageNo = pageNo;
            responseDto.Pages = 0;
            responseDto.Status = Status.Success;
            responseDto.RecordCount = 0;
            responseDto.StatusCode = StatusCodes.Status200OK;

            return responseDto;
        }


        //When records available, add pagination on that records.
        pageSize = pageSize == 0 ? _resultPerPage : pageSize;

        responseDto.Result?.ToList().ForEach(model =>
        {
            model.UserName = IEncryptionService.Decrypt(model.UserName, model.HashString);
            model.ConnectionString = IEncryptionService.Decrypt(model.ConnectionString, model.HashString);
        }
     );

        var fetchRecords =
           modelRecords.ToPagedList(pageNo == 0 ? 1 : pageNo, pageSize == 0 ? _resultPerPage : pageSize);


        //watch.Stop();
        responseDto = new(_httpContextAccessor)
        {
            DateTime = DateTime.Now,
            TimeConsumption = 0,
            RecordCount = fetchRecords?.Count(),
            CurrentPageNo = fetchRecords.PageNumber, // pageNo == 0 ? 1 : pageNo,
            Pages = fetchRecords.PageCount, //(modelRecords.Count + pageSize - 1) / pageSize,
            Message = OperationSuccessful,
            MessageType = MessageType.Information,
            Status = Status.Success,
            StatusCode = StatusCodes.Status200OK,
            Result = _mapper.Map<List<ApplicationHostMasterDtoModel>>(modelRecords)
        };
        return await Task.Run(() => responseDto);
    }


    public async Task<ResponseDto<List<ApplicationHostMasterDtoModel>>> GetAll()
    {
        ResponseDto<List<ApplicationHostMasterDtoModel>>? responseDto = new(_httpContextAccessor);
        try
        {
            var records = _dlService.GetAllAsync();
            if (records == null)
            {
                return await Task.Run(() => responseDto);
            }


            responseDto.Result = _mapper.Map<List<ApplicationHostMasterDtoModel>>(records);
            responseDto.Result.ToList().ForEach(model =>
            {
                model.UserName = IEncryptionService.Decrypt(model.UserName, model.HashString);
                model.ConnectionString = IEncryptionService.Decrypt(model.ConnectionString, model.HashString);
            }
            );
            responseDto.Message = CommonMessages.OperationSuccessful;
            responseDto.MessageType = MessageType.Information;
            responseDto.Status = Status.Success;
            responseDto.RecordCount = responseDto.Result.Count();
            responseDto.StatusCode = StatusCodes.Status200OK;

        }
        catch (Exception ex)
        {
            ex.SendExceptionMailAsync().GetAwaiter().GetResult();
            responseDto.Message += ex.Message;
        }

        return await Task.Run(() => responseDto);
    }

    public async Task<ResponseDto<ApplicationHostMasterDtoModel>> DeleteAsync(string id, bool useCache = true)
    {
        ResponseDto<ApplicationHostMasterDtoModel> responseDto = new(_httpContextAccessor);

        try
        {
            if (await _dlService.GetAsync(id) == null)
            {
                responseDto.Result = null;

                responseDto.Message = CommonMessages.OperationFailed;
                responseDto.MessageType = MessageType.Warning;
                responseDto.Status = Status.Failed;
                responseDto.RecordCount = 0;
                responseDto.StatusCode = StatusCodes.Status200OK;
                responseDto.Id = id;

                return responseDto;
            }

            await _dlService.DeleteAsync();

            responseDto.Result = null;
            responseDto.Message = CommonMessages.RecordDeleted;
            responseDto.MessageType = MessageType.Information;
            responseDto.Status = Status.Success;
            responseDto.RecordCount = 1;
            responseDto.StatusCode = StatusCodes.Status200OK;
            responseDto.Id = id;

        }
        catch (Exception ex)
        {
            ex.SendExceptionMailAsync().GetAwaiter().GetResult();
            responseDto.Message += ex.Message;
        }

        return responseDto;
    }

    public async Task<ResponseDto<ApplicationHostMasterDtoModel>> UpdateAsync(string id, ApplicationHostMasterDtoModel dto,
        bool useCache = true)
    {

        ResponseDto<ApplicationHostMasterDtoModel> responseDto = new(_httpContextAccessor);
        try
        {
            var modelRecord = await _dlService.GetAsync(id);
            if (modelRecord == null)
            {
                return responseDto;
            }

            modelRecord = _mapper.Map<ApplicationHostMasterModel>(dto);

            var username = IEncryptionService.Encrypt(dto.UserName, _hashString);
            var connectionString = IEncryptionService.Encrypt(dto.ConnectionString, _hashString);
            modelRecord.Update(_userId);
            modelRecord = await _dlService.UpdateAsync(modelRecord);

            var cacheResult = await UpdateCacheEntry<ApplicationHostMasterModel>(modelRecord, isNew: false);
            if (cacheResult.status == false)
            {
                await GetAll(1, _resultPerPage, true);
            }

            responseDto.Result = _mapper.Map<ApplicationHostMasterDtoModel>(modelRecord);

            responseDto.UserName = string.Empty;
            responseDto.Message = CommonMessages.OperationSuccessful;
            responseDto.MessageType = MessageType.Information;
            responseDto.Status = Status.Success;
            responseDto.RecordCount = 1;
            responseDto.StatusCode = StatusCodes.Status200OK;
            responseDto.Id = modelRecord.Id;
        }
        catch (Exception ex)
        {
            ex.SendExceptionMailAsync().GetAwaiter().GetResult();
            responseDto.Message += ex.Message;
        }

        return responseDto;
    }

}
