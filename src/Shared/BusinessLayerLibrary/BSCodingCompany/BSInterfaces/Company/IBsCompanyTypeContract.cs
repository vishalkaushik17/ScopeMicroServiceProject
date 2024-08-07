using GenericFunction.ResultObject;
using ModelTemplates.DtoModels.Company;

namespace BSCodingCompany.BSInterfaces.Company;

public interface IBsCompanyTypeContract
{
    /// <summary>
    /// Get all records for company types.
    /// </summary>
    /// <returns>records</returns>
    Task<ResponseDto<List<CompanyTypeDtoModel>>> GetAll();
    Task<ResponseDto<CompanyTypeDtoModel>> Get(string id);

}
