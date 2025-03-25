using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;
using HotelManagementAPI.Entities.Enums;

namespace HotelManagementAPI.Entities.Models;

[Table("rooms", Schema = "public")]
public class Room
{
    [Key]
    [Required]
    [Column("number")]
    public required int Number { get; set; }
    
    [Required]
    [Column("floor")]
    public required int Floor { get; set; }
    
    [Required]
    [Column("type")]
    public required ERoomType Type { get; set; }
    
    [Required]
    [Column("status")]
    public required ERoomStatus Status { get; set; }
    
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public UniqueRoom UniqueRoom { get; set; } = null!;
}