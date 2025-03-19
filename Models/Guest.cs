using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    [Table("guest")]
    public class Guest
    {
        [Key]
        [Column("guest_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GuestId { get; set; }
        
        [Column("full_name")]
        [StringLength(255)]
        public string FullName { get; set; }
        
        [Column("contact_info")]
        [StringLength(255)]
        public string ContactInfo { get; set; }
        
        // Навигационные свойства
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<OrderedService> OrderedServices { get; set; }
    }
}