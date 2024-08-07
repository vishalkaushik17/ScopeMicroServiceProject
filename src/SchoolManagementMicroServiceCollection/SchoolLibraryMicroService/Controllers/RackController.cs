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
public class RackController : ApiBaseController
{
    private readonly IBsLibraryRackContract _bsService;

    public RackController(IBsLibraryRackContract bsService, ITrace trace) : base(trace)
    {
        _bsService = bsService;
    }


    [HttpGet]
    [Route("Get")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<LibraryRackDtoModel>> Get(string id, string roomId, bool useCache)
    {
        return await _bsService.Get(id, roomId, useCache);
    }

    [HttpGet]
    [Route("GetAll")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<List<LibraryRackDtoModel>>> GetAll(string roomId, int pageNo = 0, int pageSize = 0, bool useCache = true)
    {
        return await _bsService.GetAll(roomId, pageNo, pageSize, useCache);

    }

    [HttpPost]
    [Route("Save")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<LibraryRackDtoModel>> Save(LibraryRackDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.AddAsync(dtoModel, useCache);
    }

    [HttpPost]
    [Route("Update")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<LibraryRackDtoModel>> Update(string id, LibraryRackDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.UpdateAsync(id, dtoModel, useCache);
    }

    [HttpGet]
    [Route("Delete")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<LibraryRackDtoModel>> Delete(string id, string roomId, bool useCache = true)
    {
        return await _bsService.DeleteAsync(id, roomId, useCache);
    }

}
