using GenericFunction.ResultObject;
using ModelTemplates.Master.Company;

namespace BSCodingCompany.BSInterfaces.Company;

public interface IBsCompanyMasterContract
{
    /// <summary>
    /// Get all records for company types.
    /// </summary>
    /// <returns>records</returns>
    Task<ResponseDto<IQueryable<CompanyMasterModel>>> GetReferenceCodes();



}
