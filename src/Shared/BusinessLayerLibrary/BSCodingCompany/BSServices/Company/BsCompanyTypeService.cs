using AutoMapper;
using BSCodingCompany.BaseClass;
using BSCodingCompany.BSInterfaces.Company;
using DataCacheLayer.CacheRepositories.Interfaces;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using ModelTemplates.DtoModels.Company;

namespace BSCodingCompany.BSServices.Company;

public sealed class BsCompanyTypeService : BaseBusinessLayer, IBsCompanyTypeContract
{
    public BsCompanyTypeService(ICacheContract _cache, IHttpContextAccessor httpContextAccessor, IMapper iMapper, ICacheContract cache, ITrace trace)
        : base(iMapper, httpContextAccessor, trace, cache)
    {
    }

    public Task<ResponseDto<CompanyTypeDtoModel>> Get(string id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto<List<CompanyTypeDtoModel>>> GetAll()
    {
        throw new NotImplementedException();
    }
}
