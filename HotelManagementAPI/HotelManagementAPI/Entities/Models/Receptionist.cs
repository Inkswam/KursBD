using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementAPI.Entities.Models;

[Table("receptionists", Schema = "public")]
public class Receptionist
{
    [Key]
    [Required]
    [Column("email")]
    [MaxLength(254)]
    public required string Email { get; set; }

    public User User { get; set; } = null!;
}