using GenericFunction;
using GenericFunction.Enums;
using Microsoft.AspNetCore.Identity;
using ModelTemplates.Core.GenericModel;
using ModelTemplates.EntityModels.Company;
using ModelTemplates.Master.Company;
using System.Globalization;

namespace ModelTemplates.EntityModels.Application;

public sealed class ApplicationUser : IdentityUser, IGenericContract
{
    public ApplicationUser()
    {


        RecordStatus = EnumRecordStatus.Active;
        IsEditable = false;
        CreatedOn = DateTime.Now;
        ConcurrencyStamp = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        Id = Guid.NewGuid().ToString("D");
        AccessFailedCount = 0;
        TwoFactorEnabled = false;
        EmailConfirmed = false;
        PhoneNumber = "0";
        PhoneNumberConfirmed = false;
        LockoutEnabled = false;
        AcceptTerms = false;


        RefreshTokens = new List<RefreshToken>();

    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PersonalEmailId { get; set; }

    //for refresh token 
    public bool AcceptTerms { get; set; }
    //public Role Role { get; set; }
    public string? VerificationToken { get; set; }
    public DateTime? Verified { get; set; }
    public bool IsVerified => Verified.HasValue || PasswordReset.HasValue;
    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpires { get; set; }
    public DateTime? PasswordReset { get; set; }
    //public DateTime CreatedOn { get; set; }
    //public DateTime? Updated { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }//= new List<RefreshToken>();

    public bool OwnsToken(string token)
    {
        return this.RefreshTokens?.Find(x => x.Token == token) != null;
    }

    //
    public EnumRecordStatus RecordStatus { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string UserId { get; set; } //which user has worked on this record. it is for tracing
    public bool IsEditable { get; set; }
    public string CompanyId { get; set; }
    public CompanyMasterModel Company { get; set; }
    public string CompanyTypeId { get; set; }
    public CompanyTypeModel CompanyType { get; set; }
    public void Save(string userId)
    {
        UserId = UserId;
    }

    public void ConfirmEmail()
    {
        EmailConfirmed = true;
    }
    public void Save(string userId, string userName, string companyId, string companyTypeId, string email, string firstName, string lastName, string personalEmailId)
    {
        UserId = userId;
        UserName = userName;
        NormalizedUserName = string.IsNullOrWhiteSpace(NormalizedUserName) ? userName.ToUpper() : NormalizedUserName.ToUpper();
        NormalizedEmail = string.IsNullOrWhiteSpace(NormalizedEmail) ? email.ToLower() : NormalizedEmail.ToLower();

        CompanyId = companyId;
        CompanyTypeId = companyTypeId;
        Email = email;

        FirstName = firstName.ToCamelCase();
        LastName = lastName.ToCamelCase();
        PersonalEmailId = personalEmailId.ToCamelCase();

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