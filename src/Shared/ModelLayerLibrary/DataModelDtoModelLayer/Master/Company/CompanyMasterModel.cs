using GenericFunction;
using GenericFunction.Enums;

namespace ModelTemplates.Master.Company;
public class CompanyMasterModel : CompanyTemplate
{
    public CompanyMasterProfileModel CompanyMasterProfile { get; set; }

    /// <summary>
    /// It is a default method for first time school registered as a demo
    /// </summary>
    /// <param name="userId"></param>
    public new void Save(string userId)
    {
        Name = Name.ToCamelCase();
        Email = Email.ToLower();
        EnrollmentDate = DateTime.Now;
        SuffixDomain = SuffixDomain.ToLower();
        UserId = userId;
        IsDemoExpired = false;
        IsDemoMode = true;
    }

    public override void Save(string? id, string userid, DateTime docDateTime, DateTime accountExpireDateTime,
        DateTime demoExpireDateTime, string companyTypeId, string suffixDomain, string name, string email,
        DateTime enrollmentDate, bool isDemoMode, string referenceCode, bool isDemoExpired,
        bool isEditable, EnumRecordStatus recordStatus, string website, string demoRequestId)
    {
        Id = id ?? this.Id;
        UserId = userid;
        CreatedOn = docDateTime;
        AccountExpire = accountExpireDateTime;
        DemoExpireDate = demoExpireDateTime;
        CompanyTypeId = companyTypeId;
        SuffixDomain = suffixDomain.ToLower().Replace("@",""); //Eg @domain.name
        Website = website.ToLower(); //Eg @domain.name
        Name = name.ToCamelCase();
        Email = email.ToLower();
        EnrollmentDate = enrollmentDate;
        IsDemoMode = isDemoMode;
        ReferenceCode = referenceCode;
        IsDemoExpired = isDemoExpired;
        IsEditable = isEditable;
        RecordStatus = recordStatus;
        DemoRequestId = demoRequestId;
        
    }

    public override void SetUser(string userId, string cTypeId)
    {
        UserId = userId;
        CompanyTypeId = cTypeId;
    }
}