using GenericFunction.Enums;
using Microsoft.AspNetCore.Identity;
using ModelTemplates.Core.GenericModel;
using ModelTemplates.Master.Company;

namespace ModelTemplates.EntityModels.Application;

public class UserRoles : IdentityRole, IGenericContract
{



    public UserRoles(string? userId = "DEFAULT")
    {
        UserId = userId ?? "DEFAULT";
    }




    public EnumRecordStatus RecordStatus { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string UserId { get; set; }
    public bool IsEditable { get; set; }

    public CompanyMasterModel Company { get; set; }
    public string CompanyId { get; set; }

    public void Save(string userId)
    {
        UserId = userId;
    }
    public void Save(string userId, string roleName)
    {
        UserId = userId;
        RecordStatus = EnumRecordStatus.Active;
        IsEditable = false;
        CreatedOn = DateTime.Now;
        ConcurrencyStamp = DateTime.Now.ToString();
        Name = roleName;
        NormalizedName = roleName.ToUpper();
        Id = Guid.NewGuid().ToString("D");

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