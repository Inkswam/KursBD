using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models
{
    [Table("orderedservice")]
    public class OrderedService
    {
        [Key]
        [Column("order_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        
        [Column("guest_id")]
        public int GuestId { get; set; }
        
        [Column("service_id")]
        public int ServiceId { get; set; }
        
        [Column("order_date")]
        public DateTime OrderDate { get; set; }
        
        [Column("price")] public decimal Price { get; set; }
        
        // Навигационные свойства
        [ForeignKey("GuestId")]
        public virtual Guest Guest { get; set; }
        
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }
    }
}