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
public class BookshelfController : ApiBaseController
{
    private readonly IBsLibraryBookshelfContract _bsService;
    public BookshelfController(IBsLibraryBookshelfContract bsService, ITrace trace) : base(trace)
    {
        _bsService = bsService;
    }


    [HttpGet]
    [Route("Get")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<LibraryBookshelfDtoModel>> Get(string id, string rackid, bool useCache = true)
    {
        return await _bsService.Get(id, rackid, useCache);
    }

    [HttpGet]
    [Route("GetAll")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<List<LibraryBookshelfDtoModel>>> GetAll(string rackId, int pageNo = 0, int pageSize = 0, bool useCache = true)
    {
        return await _bsService.GetAll(rackId, pageNo, pageSize, useCache);

    }

    [HttpPost]
    [Route("Save")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<LibraryBookshelfDtoModel>> Save(LibraryBookshelfDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.AddAsync(dtoModel, useCache);
    }

    [HttpPost]
    [Route("Update")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<LibraryBookshelfDtoModel>> Update(string id, LibraryBookshelfDtoModel dtoModel, bool useCache = true)
    {
        return await _bsService.UpdateAsync(id, dtoModel, useCache);


    }

    [HttpGet]
    [Route("Delete")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<LibraryBookshelfDtoModel>> Delete(string id, string rackId, bool useCache = true)
    {
        return await _bsService.DeleteAsync(id, rackId, useCache);
    }

}
