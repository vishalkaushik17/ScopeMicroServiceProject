using GenericFunction.Enums;
using ModelTemplates.Core.GenericModel;
using ModelTemplates.EntityModels.Company;

namespace ModelTemplates.Master.Company;

public abstract class CompanyTemplate : BaseTemplate
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
    public string SuffixDomain { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public DateTime DemoExpireDate { get; set; }
    public DateTime? AccountExpire { get; set; }
    public CompanyTypeModel CompanyTypeEntityModel { get; set; }
    public string CompanyTypeId { get; set; } = string.Empty;
    public string ReferenceCode { get; set; } = "Default";
    public string ParentReferenceCode { get; set; } = "Default";
    public bool IsDemoExpired { get; set; } = false;
    public bool IsDemoMode { get; set; } = true;
    public string DemoRequestId { get; set; } = string.Empty;

    public abstract void Save(string? id, string userid, DateTime docDateTime, DateTime accountExpireDateTime,
        DateTime demoExpireDateTime, string companyTypeId, string suffixDomain, string name, string email,
        DateTime enrollmentDate, bool isDemoMode, string referenceCode, bool isDemoExpired,
        bool isEditable, EnumRecordStatus recordStatus, string website, string demoRequestId);
    public abstract void SetUser(string userId, string cTypeId);
}
