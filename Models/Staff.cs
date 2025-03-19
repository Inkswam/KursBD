using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HotelManagementSystem.Models
{
    [Table("staff")]
    public class Staff
    {
        [Key]
        [Column("staff_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StaffId { get; set; }
        
        [Column("full_name")]
        [StringLength(255)]
        public string FullName { get; set; }
        
        [Column("position")]
        [StringLength(100)]
        public string Position { get; set; }
        
        [Column("work_schedule")]
        [StringLength(255)]
        public string WorkSchedule { get; set; }
        
        // Навигационные свойства
        public virtual ICollection<Cleaning> Cleanings { get; set; }
    }
}