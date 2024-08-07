namespace ModelTemplates.RequestNResponse.Accounts;

/// <summary>
/// This class is responsible to authentication user properties for token and httpContext session
/// </summary>
public class ApplicationUserDtoModel
{
    public string UserId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public List<string?> Roles { get; set; } = new();
    public string ClientEmail { get; set; } = string.Empty;
    public string CompanyTypeId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PersonalEmailId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string WebSite { get; set; } = string.Empty;
    public string TokenSessionId { get; set; } = string.Empty;
    public string TokenScopeId { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string JwtToken { get; set; } = string.Empty;

    public bool IsVerified { get; set; } 
    public bool IsSuccess { get; set; }
    public DateTime CreatedOn { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
    //public string CompanyId { get; set; } = string.Empty;
    //public string CompanyName { get; set; } = string.Empty;

    //public string UserId { get; set; } = string.Empty;
    //public string Title { get; set; } = string.Empty;
    //public string PersonalEmailId { get; set; } = string.Empty;
    //public string FirstName { get; set; } = string.Empty;
    //public string LastName { get; set; } = string.Empty;
    //public string Email { get; set; } = string.Empty;
    //public string WebSite { get; set; } = string.Empty;
    //public string UserName { get; set; } = string.Empty;
    //public List<string> Roles { get; set; } = new List<string>();
    //public List<string> Errors { get; set; } = new List<string>();

    //public DateTime CreatedOn { get; set; } 
    //public DateTime? Updated { get; set; } 
    //public bool IsVerified { get; set; } 
    //public bool IsSuccess { get; set; }
    //public string JwtToken { get; set; } = string.Empty;

    //[JsonIgnore] // refresh token is returned in http only cookie
    //public string RefreshToken { get; set; } = string.Empty;

    //public string Message { get; set; } = string.Empty;

    //public string SessionId { get; set; } = string.Empty;
    //public string ScopeId { get; set; } = string.Empty;

}
