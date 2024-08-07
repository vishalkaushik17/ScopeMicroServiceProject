using System.ComponentModel.DataAnnotations;

namespace VModelLayer;

public class LoginModel
{
    [Required(ErrorMessage = "Username is required")]
    [EmailAddress]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
    public string __RequestVerificationToken { get; set; }

    public string Url { get; set; } = string.Empty;
    public string? Message { get; set; }
    public string? ScopeId { get; set; }

}