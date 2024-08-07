using GenericFunction.Enums;
using Microsoft.AspNetCore.Identity;
using ModelTemplates.Core.GenericModel;
using ModelTemplates.Master.Company;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelTemplates.EntityModels.Application;

public class ApplicationUserToken : IdentityUserToken<string>, IGenericContract
{
    [NotMapped]
    public string Id { get; set; }
    public EnumRecordStatus RecordStatus { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public bool IsEditable { get; set; }

    public CompanyMasterModel Company { get; set; }
    public string CompanyId { get; set; }

    public void Save(string userId)
    {
        throw new NotImplementedException();
    }

    public void Delete(string userId)
    {
        throw new NotImplementedException();
    }

    public void Update(string userId)
    {
        throw new NotImplementedException();
    }
}