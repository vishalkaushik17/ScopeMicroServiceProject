namespace GenericFunction;

public class JtwTokenContainerResponse
{
    public string UserId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public List<string?> Roles { get; set; } = new();
    public string ClientEmail { get; set; } = string.Empty;
    public string CompanyTypeId { get; set; } = string.Empty;
    //public string FirstName { get; set; } = string.Empty;
    //public string LastName { get; set; } = string.Empty;
    //public string PersonalEmailId { get; set; } = string.Empty;
    //public string WebSite { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public string TokenSessionId { get; set; } = string.Empty;
    public string TokenScopeId { get; set; } = string.Empty;
    public bool IsDemoExpired { get; set; }
}