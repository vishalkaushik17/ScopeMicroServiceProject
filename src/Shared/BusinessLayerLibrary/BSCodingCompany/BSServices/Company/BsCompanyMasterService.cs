using AutoMapper;
using BSCodingCompany.BaseClass;
using BSCodingCompany.BSInterfaces.Company;
using DataCacheLayer.CacheRepositories.Interfaces;
using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using ModelTemplates.Master.Company;

namespace BSCodingCompany.BSServices.Company;

public class BsCompanyMasterService : BaseBusinessLayer, IBsCompanyMasterContract
{
    public BsCompanyMasterService(ICacheContract _cache, IHttpContextAccessor httpContextAccessor, IMapper iMapper,ICacheContract cache,ITrace trace)
        : base(iMapper, httpContextAccessor,trace,cache)
    {
    }
    public Task<ResponseDto<IQueryable<CompanyMasterModel>>> GetReferenceCodes()
    {
        throw new NotImplementedException();
    }
}
