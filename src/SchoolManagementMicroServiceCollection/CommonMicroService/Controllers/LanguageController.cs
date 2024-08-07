using Asp.Versioning;
using AutoMapper;
using BSLayerSchool.BSInterfaces.CommonContracts;
using CommonMicroService.Controllers.Base;
using GenericFunction.Constants.Authorization;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelTemplates.DtoModels.CommonDtoModels;
using ModelTemplates.EntityModels.Application;
using SharedLibrary.Services;
using SharedLibrary.Services.CustomFilters;

namespace CommonMicroService.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class LanguageController : ApiBaseController
{
    private readonly IBsLanguageContract _bsService;

    public LanguageController(IBsLanguageContract bsService, ITrace trace) : base()
    {
        _bsService = bsService;
    }

    [HttpGet]
    [Route("Get")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<LanguageDtoModel>> Get(string id, bool UseCache = true)
    {
        return await _bsService.Get(id, UseCache);
    }

    [HttpGet]
    [Route("GetAll")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<List<LanguageDtoModel>>> GetAll(int pageNo = 0, int pageSize = 0, bool UseCache = true)
    {

        return await _bsService.GetAll(pageNo, pageSize,UseCache);
    }

    [HttpPost]
    [Route("Save")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<LanguageDtoModel>> Save(LanguageDtoModel dtoModel, bool UseCache = true)
    {
        return await _bsService.AddAsync(dtoModel, UseCache);
    }

    [HttpPost]
    [Route("Update")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<LanguageDtoModel>> Update(string id, LanguageDtoModel dtoModel, bool UseCache = true)
    {
        return await _bsService.UpdateAsync(id, dtoModel, UseCache);
    }

    [HttpGet]
    [Route("Delete")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<LanguageDtoModel>> Delete(string id, bool UseCache = true)
    {
        return await _bsService.DeleteAsync(id,UseCache);
    }
}
