using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using HotelManagementAPI.Entities.Enums;

namespace HotelManagementAPI.Entities.Models;

[Table("reservation", Schema = "public")]
public class Reservation
{
    [Key]
    [Required]
    [Column("id")]
    public required Guid Id { get; set; }
    
    [Required]
    [Column("guest_email")]
    [MaxLength(254)]
    public required string GuestEmail { get; set; }
    
    [Required]
    [Column("room_number")]
    public required int RoomNumber { get; set; }
    
    [Required]
    [Column("room_type")]
    public required ERoomType RoomType { get; set; }
    
    [Required]
    [Column("check_in_date")]
    public required DateOnly CheckInDate { get; set; }
    
    [Required]
    [Column("check_out_date")]
    public required DateOnly CheckOutDate { get; set; }
    
    [Required]
    [Column("status")]
    public required EReservationStatus Status { get; set; }
    
    [Required]
    [Column("services")]
    [MaxLength(500)]
    public required string Services { get; set; }
    
    public Guest Guest { get; set; } = null!;
    public Room Room { get; set; } = null!;
    public Payment? Payment { get; set; }
    
    
}