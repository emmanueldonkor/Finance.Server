
using System.ComponentModel.DataAnnotations;

namespace server.Entities;

public class User
{
   public int Id { get; set; } 
   [StringLength(50)]
   public required string Username { get; set; }
   public required string Password {get; set; }
   [EmailAddress]
   public string? Email {get; set;} 
   public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

   public string? ExternalId {get; set;}
   public string? ExternalType {get; set;} 
}