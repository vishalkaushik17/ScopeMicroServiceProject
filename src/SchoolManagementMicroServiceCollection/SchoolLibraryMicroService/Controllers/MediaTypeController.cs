using Asp.Versioning;
using BSLayerSchool.BSInterfaces.SchoolLibraryContracts;
using GenericFunction.Constants.Authorization;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Mvc;
using ModelTemplates.DtoModels.SchoolLibrary;
using SchoolLibraryMicroService.Controllers.Base;
using SharedLibrary.Services;
using SharedLibrary.Services.CustomFilters;

namespace SchoolLibraryMicroService.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class MediaTypeController : ApiBaseController
{
    private readonly IBsLibraryMediaTypeContract _bsService;
    public MediaTypeController(IBsLibraryMediaTypeContract bsService, ITrace trace) : base(trace)
    {
        _bsService = bsService;
    }


    [HttpGet]
    [Route("Get")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<LibraryMediaTypeDtoModel>> Get(string id, bool useCache = true)
    {
        return await _bsService.Get(id, useCache);
    }

    [HttpGet]
    [Route("GetAll")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<List<LibraryMediaTypeDtoModel>>> GetAll(int pageNo = 0, int pageSize = 0, bool useCache = true)
    {
        return await _bsService.GetAll(pageNo, pageSize, useCache);

    }

    [HttpPost]
    [Route("Save")]
    //[CustomAuthorize(RoleName.ErpAdmin)]
    public async Task<ResponseDto<LibraryMediaTypeDtoModel>> Save(LibraryMediaTypeDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.AddAsync(dtoModel, useCache);
    }

    [HttpPost]
    [Route("Update")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<LibraryMediaTypeDtoModel>> Update(string id, LibraryMediaTypeDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.UpdateAsync(id, dtoModel, useCache);
    }

    [HttpGet]
    [Route("Delete")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<LibraryMediaTypeDtoModel>> Delete(string id, bool useCache = true)
    {
        return await _bsService.DeleteAsync(id, useCache);
    }

}
