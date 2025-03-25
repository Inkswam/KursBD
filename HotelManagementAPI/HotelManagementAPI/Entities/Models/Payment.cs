using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelManagementAPI.Entities.Enums;

namespace HotelManagementAPI.Entities.Models;

[Table("payments", Schema = "public")]
public class Payment
{
    [Key]
    [Required]
    [Column("id")]
    public required Guid Id { get; set; }
    
    [Required]
    [Column("reservation_id")]
    public required Guid ReservationId { get; set; }
    
    [Required]
    [Column("date")]
    public required DateOnly Date { get; set; }
    
    [Required]
    [Column("amount")]
    public required long Amount { get; set; }
    
    [Required]
    [Column("payment_method")]
    public required EPaymentMethod PaymentMethod { get; set; }
    
    public Reservation Reservation { get; set; } = null!;
}