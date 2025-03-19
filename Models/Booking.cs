using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    [Table("booking")]
    public class Booking
    {
        [Key]
        [Column("booking_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }
        
        [Column("guest_id")]
        public int GuestId { get; set; }
        
        [Column("room_number")]
        public string RoomNumber { get; set; }
        
        [Column("check_in_date")]
        public DateTime CheckInDate { get; set; }
        
        [Column("check_out_date")]
        public DateTime CheckOutDate { get; set; }
        
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; }
        
        // Навигационные свойства
        [ForeignKey("GuestId")]
        public virtual Guest Guest { get; set; }
        
        [ForeignKey("RoomNumber")]
        public virtual Room Room { get; set; }
        
        public virtual ICollection<Payment> Payments { get; set; }
    }
}