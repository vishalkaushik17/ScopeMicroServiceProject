﻿using AutoMapper;
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
namespace BSLayerSchool.BSRepositories.CommonServices;

public sealed class BsDepartmentService : BaseBusinessLayer, IBsDepartmentContract
{
    private readonly IMCDepartmentContract _cacheService;

    public BsDepartmentService(IMCDepartmentContract cacheService, ICacheContract _cache, IHttpContextAccessor httpContextAccessor, IMapper iMapper, ITrace trace)
    : base(iMapper, httpContextAccessor, trace, _cache)
    {
        _cacheService = cacheService;
    }

    /// <summary>
    /// Get record
    /// </summary>
    /// <param name="id">record id</param>
    /// <param name="UseCache">true for use Redis cache, default is true</param>
    /// <returns>ResponseDto object</returns>

    public async Task<ResponseDto<DepartmentDtoModel>> GetAsync(string id, bool UseCache = true)
    {
        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started".MarkInformation());

        DepartmentModel? modelRecord = await _cacheService.GetCacheAsync(id, _modificationInDays, _clientId, UseCache);

        if (modelRecord == null)
        {
            return FailureResponse<DepartmentDtoModel>(CommonMessages.RecordNotFound);
        }


        //Send response to controller class
        return await Task.Run(() => SuccessResponse<DepartmentModel, DepartmentDtoModel>(id, modelRecord));
    }


    public async Task<ResponseDto<List<DepartmentDtoModel>>> GetAllAsync(int pageNo, int pageSize,
        bool UseCache = true)
    {

        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started".MarkInformation());

        //First check cache entry
        List<DepartmentModel>? modelRecords = new List<DepartmentModel>();

        modelRecords = await _cacheService.GetAllCacheAsync(_modificationInDays, _clientId, UseCache);


        if (modelRecords == null || modelRecords?.Count() == 0)
        {
            return FailureResponse<List<DepartmentDtoModel>>(CommonMessages.RecordNotFound);
        }

        return await Task.Run(() => SuccessResponse<DepartmentModel, List<DepartmentDtoModel>>(modelRecords));
    }


    public async Task<ResponseDto<DepartmentDtoModel>> AddAsync(DepartmentDtoModel dto, bool UseCache = true)
    {

        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for adding new record!".MarkInformation());

        DepartmentModel modelRecord = new DepartmentModel();
        modelRecord = _mapper.Map<DepartmentModel>(dto);

        //find record from repository
        //check record availability by name
        if (await FindExistingRecordAsync(modelRecord.Name, modelRecord.Id, _modificationInDays, _clientId, UseCache))
        {
            return DuplicateEntryResponse<DepartmentDtoModel>("Department Name", dto.Name);
        }

        //Do all business operation here


        //Cache response from repository and sent back
        modelRecord = await _cacheService.AddCacheAsync(modelRecord, _clientId, UseCache);

        if (string.IsNullOrWhiteSpace(modelRecord.Id))
        {
            return FailureResponse<DepartmentDtoModel>(CommonMessages.OperationFailed);
        }

        modelRecord.CheckIsEditable(_modificationInDays);

        return await Task.Run(() => SuccessResponse<DepartmentModel, DepartmentDtoModel>(modelRecord.Id, modelRecord));
    }

    public async Task<ResponseDto<DepartmentDtoModel>> UpdateAsync(string id, DepartmentDtoModel dto,
        bool UseCache = true)
    {

        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for updating record!".MarkInformation());

        //check record availability by name
        if (await FindExistingRecordAsync(dto.Name, dto.Id, _modificationInDays, _clientId, UseCache))
        {
            return DuplicateEntryResponse<DepartmentDtoModel>("Department Name", dto.Name);
        }

        //check record availability by id
        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), $"Checking records for id {id}");
        DepartmentModel? modelRecord = await _cacheService.GetCacheAsync(id, _modificationInDays, _clientId, UseCache);

        //When no record available.
        if (modelRecord == null || modelRecord.Id != dto.Id)
        {
            return FailureResponse<DepartmentDtoModel>(CommonMessages.RecordNotFound);
        }
        //when no permission for updating record
        else if (modelRecord.IsEditable == false)
        {
            //watch.Stop();
            return FailureResponse<DepartmentDtoModel>(CommonMessages.NotPermitted);
        }

        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), $"Record found updating!");
        modelRecord = _mapper.Map<DepartmentModel>(dto);

        await _cacheService.UpdateCacheAsync(modelRecord, _clientId, UseCache);
        modelRecord.CheckIsEditable(_modificationInDays);
        return await Task.Run(() => SuccessResponse<DepartmentModel, DepartmentDtoModel>(modelRecord.Id, modelRecord));
    }

    public async Task<ResponseDto<DepartmentDtoModel>> DeleteAsync(string id, bool UseCache = true)
    {

        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), "Process started for deleting record!".MarkInformation());

        //check record availability by id
        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationStart.MarkProcess(), $"Checking records for id {id}");
        DepartmentModel modelRecord = await _cacheService.GetCacheAsync(id, _modificationInDays, _clientId, UseCache);

        if (modelRecord == null)
        {
            return FailureResponse<DepartmentDtoModel>(CommonMessages.RecordNotFound);
        }
        else if (!modelRecord.IsEditable)
        {
            FailureResponse<DepartmentDtoModel>(CommonMessages.NotPermitted);
        }
        _trace.AddTrace(this.NameOfClass(), ExtensionMethods.GetCurrentMethodName(), _IsTracingRequired, OperationEnd.MarkProcess(), $"Record found for marking it deleted!");

        //check if record is used in other table
        bool recordUsed = await _cacheService.IsRecordInUseAsync(id);//.AnyAsync(m => m.LibraryId == id);

        if (recordUsed)
        {
            return FailureResponse<DepartmentDtoModel>(CommonMessages.RecordInUsed);
        }


        modelRecord.RecordStatus = await _cacheService.DeleteCacheAsync(id, modelRecord, _modificationInDays, UseCache);


        if (modelRecord.RecordStatus != GenericFunction.Enums.EnumRecordStatus.Deleted)
        {
            return FailureResponse<DepartmentDtoModel>(CommonMessages.OperationFailed);
        }

        return await Task.Run(() => SuccessResponse<DepartmentModel, DepartmentDtoModel>(modelRecord.Id, modelRecord));
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
        List<DepartmentModel>? records = await _cacheService.GetAllCacheAsync(_modificationInDays, _clientId, useCache);
        //if no records inserted in db or cache
        if (records == null || records?.Count == 0)
            return false;

        //id is null while adding new record, don't change the condition
        if (string.IsNullOrWhiteSpace(id))
        {
            //New Record

            return await Task.Run(() => records.Any(m => m.Name.ToUpper() == fieldName.ToUpper()
                                                         && m.RecordStatus == EnumRecordStatus.Active));
        }
        else
        {
            //when record id is available it mean we are updating record, so checking 
            //condition for existing record.
            return await Task.Run(() => records.Any(m => m.Name.ToUpper() == fieldName.ToUpper()
                                                         && m.RecordStatus == EnumRecordStatus.Active
                                                         && m.Id != id));
        }
    }

}

