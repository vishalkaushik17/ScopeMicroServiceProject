//namespace WebApi.Entities;

//using Microsoft.EntityFrameworkCore;
//using System.ComponentModel.DataAnnotations;

//[Owned]
//public class RefreshToken
//{
//    [Key]
//    public int Id { get; set; }
//    public Account Account { get; set; }
//    public string Token { get; set; }
//    public DateTime Expires { get; set; }
//    public DateTime CreatedOn { get; set; }
//    public string CreatedByClientId { get; set; }
//    public DateTime? Revoked { get; set; }
//    public string RevokedByIp { get; set; }
//    public string ReplacedByToken { get; set; }
//    public string ReasonRevoked { get; set; }
//    public bool IsExpired => DateTime.UtcNow >= Expires;
//    public bool IsRevoked => Revoked != null;
//    public bool IsActive => Revoked == null && !IsExpired;
//}