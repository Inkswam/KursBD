using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementAPI.Entities.Models;

[Table("blacklist", Schema = "public")]
public class Blacklist
{
    [Key]
    [Required]
    [Column("email")]
    [MaxLength(254)]
    public required string Email { get; set; }

    [Column("reason")]
    [MaxLength(1000)]
    public string? Reason { get; set; }
}