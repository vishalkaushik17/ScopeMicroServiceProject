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
public class RoomController : ApiBaseController
{
    private readonly IBsLibraryRoomContract _bsService;
    public RoomController(IBsLibraryRoomContract bsService, ITrace trace) : base(trace)
    {
        _bsService = bsService;
    }


    [HttpGet]
    [Route("Get")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<LibraryRoomDtoModel>> Get(string id, string libraryId, bool UseCache = true)
    {
        return await _bsService.Get(id, libraryId, UseCache);
    }

    [HttpGet]
    [Route("GetAll")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin, RoleName.Librarian, RoleName.AsstLibrarian)]
    public async Task<ResponseDto<List<LibraryRoomDtoModel>>> GetAll(string libraryId, int pageNo = 0, int pageSize = 0, bool UseCache = true)
    {
        return await _bsService.GetAll(libraryId, pageNo, pageSize, UseCache);

    }

    [HttpPost]
    [Route("Save")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<LibraryRoomDtoModel>> Save(LibraryRoomDtoModel dtoModel, bool UseCache = true)
    {
        return await _bsService.AddAsync(dtoModel, UseCache);
    }

    [HttpPost]
    [Route("Update")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<LibraryRoomDtoModel>> Update(string id, LibraryRoomDtoModel dtoModel, bool UseCache = true)
    {
        return await _bsService.UpdateAsync(id, dtoModel, UseCache);
    }

    [HttpGet]
    [Route("Delete")]
    [CustomAuthorize(RoleName.Administrator, RoleName.ErpAdmin)]
    public async Task<ResponseDto<LibraryRoomDtoModel>> Delete(string id, string libraryId, bool UseCache = true)
    {
        return await _bsService.DeleteAsync(id, libraryId, UseCache);
    }

}
