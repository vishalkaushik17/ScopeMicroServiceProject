using Asp.Versioning;
using BSLayerSchool.BSInterfaces.SchoolLibraryContracts;
using GenericFunction.Constants.Authorization;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Mvc;
using ModelTemplates.DtoModels.SchoolLibrary;
using SchoolLibraryMicroService.Controllers.Base;

//using SchoolLibraryMicroService.Controllers.Base;
using SharedLibrary.Services;
using SharedLibrary.Services.CustomFilters;

namespace SchoolLibraryMicroService.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SectionController : ApiBaseController
{
    private readonly IBsLibrarySectionContract _bsService;
    public SectionController(IBsLibrarySectionContract bsService, ITrace trace) : base(trace)
    {
        _bsService = bsService;
    }


    [HttpGet]
    [Route("Get")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<LibrarySectionDtoModel>> Get(string id, string roomId, bool useCache = true)
    {
        return await _bsService.Get(id, roomId, useCache);
    }

    [HttpGet]
    [Route("GetAll")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<List<LibrarySectionDtoModel>>> GetAll(string roomId, int pageNo = 0, int pageSize = 0, bool useCache = true)
    {
        return await _bsService.GetAll(roomId, pageNo, pageSize, useCache);
    }

    [HttpPost]
    [Route("Save")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<LibrarySectionDtoModel>> Save(LibrarySectionDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.AddAsync(dtoModel, useCache);
    }

    [HttpPost]
    [Route("Update")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<LibrarySectionDtoModel>> Update(string id, LibrarySectionDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.UpdateAsync(id, dtoModel, useCache);
    }

    [HttpGet]
    [Route("Delete")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<LibrarySectionDtoModel>> Delete(string id, string roomId, bool useCache = true)
    {
        return await _bsService.DeleteAsync(id, roomId, useCache);
    }

}
