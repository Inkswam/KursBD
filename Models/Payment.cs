using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models
{
    [Table("payment")]
    public class Payment
    {
        [Key]
        [Column("payment_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }
        
        [Column("booking_id")]
        public int BookingId { get; set; }
        
        [Column("amount")]
        public decimal Amount { get; set; }
        
        
        [Column("payment_date")]
        public DateTime PaymentDate { get; set; }
        
        [Column("payment_method")]
        [StringLength(50)]
        public string PaymentMethod { get; set; }
        
        // Навигационные свойства
        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }
    }
}