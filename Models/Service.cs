using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    [Table("service")]
    public class Service
    {
        [Key]
        [Column("service_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceId { get; set; }
        
        [Column("name")]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Column("description")]
        public string Description { get; set; }
        
        [Column(TypeName = "numeric(10,2)")]
        public decimal Price { get; set; }
        
        // Навигационные свойства
        public virtual ICollection<OrderedService> OrderedServices { get; set; }
    }
}