using ModelTemplates.Core.GenericModel;
using ModelTemplates.Master.Company;
using System.Security.Cryptography;

namespace ModelTemplates.EntityModels.Application;

//[Owned]
public class RefreshToken : BaseTemplate
{


    public void GenerateRefreshToken(string userid,string companyId)
    {
        Id = string.IsNullOrWhiteSpace(Id) ? Guid.NewGuid().ToString("D") : Id;
        // token is a cryptographically strong random sequence of values
        Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        // token is valid for 7 days
        Expires = DateTime.UtcNow.AddDays(7);
        Created = DateTime.UtcNow;
        UserId = userid;
        CompanyId = companyId;
    }
    public ApplicationUser? User { get; set; } = new();
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; } = string.Empty;
    public DateTime? Revoked { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? ReasonRevoked { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsRevoked => Revoked != null;
    public bool IsActive => Revoked == null && !IsExpired;
    public string CompanyId { get; set; } = string.Empty;
    public CompanyMasterModel? CompanyMasterEntityModel { get; set; } = new();

}