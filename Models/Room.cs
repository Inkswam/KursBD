using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    [Table("room")]
    public class Room
    {
        [Key]
        [Column("room_number")]
        public string RoomNumber { get; set; }
        
        [Column("category")]
        [StringLength(50)]
        public string Category { get; set; }
        
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; }
        
        [Column("price_per_night")] public decimal PricePerNight { get; set; }
        
        // Навигационные свойства
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Cleaning> Cleanings { get; set; }
    }
}