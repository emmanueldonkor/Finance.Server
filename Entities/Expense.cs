using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Entities;

public class Expense
{
  public int Id { get; set; }
  [StringLength(50)]
  public required string Description { get; set; }
  [Range(1,10000)]
  public required decimal Amount { get; set; }
   public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

  [ForeignKey("UserId")]
  public User?  User {get; set;}
}