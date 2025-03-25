using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementAPI.Entities.Models;

[Table("guests", Schema = "public")]
public class Guest
{
    [Key]
    [Required]
    [Column("email")]
    [MaxLength(254)]
    public required string Email { get; set; }
    
    [Column("history")]
    [MaxLength(1000)]
    public string? History { get; set; }
    
    public User User { get; set; } = null!;
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}