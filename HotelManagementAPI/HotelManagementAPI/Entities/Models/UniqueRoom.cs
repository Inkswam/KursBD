using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelManagementAPI.Entities.Enums;

namespace HotelManagementAPI.Entities.Models;

[Table("unique_room", Schema = "public")]
public class UniqueRoom
{
    [Key]
    [Required]
    [Column("room_type")]
    public required ERoomType RoomType { get; set; }
    
    [Required]
    [Column("capacity")]
    [Range(1, 10)]
    public required int Capacity { get; set; }
    
    [Required]
    [Column("price")]
    public long Price { get; set; }
    
    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}