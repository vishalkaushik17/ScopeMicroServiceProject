using ModelTemplates.Core.GenericModel;
using ModelTemplates.Master.Company;

namespace ModelTemplates.EntityModels.AppConfig;

public sealed class AppDBHostVsCompanyMaster : BaseTemplate
{
    public ApplicationHostMasterModel ApplicationHostMaster { get; set; }
    public string AppHostId { get; set; }
    public CompanyMasterModel CompanyMaster { get; set; }
    public string CompanyMasterId { get; set; }
}