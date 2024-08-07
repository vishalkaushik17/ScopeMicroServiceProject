using System.ComponentModel.DataAnnotations;

namespace ModelTemplates.RequestNResponse.Accounts;

public class AuthenticateRequest
{
    [Required]
    [EmailAddress]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}