using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models
{
    [Table("cleaning")]
    public class Cleaning
    {
        [Key]
        [Column("cleaning_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CleaningId { get; set; }

        [Column("room_number")]
        public string RoomNumber { get; set; }

        [Column("staff_id")]
        public int StaffId { get; set; }

        [Column("cleaning_date")]
        public DateTime CleaningDate { get; set; }

        [Column("cleaning_time")]
        public string CleaningTime { get; set; } // Added CleaningTime

        [Column("cleaning_type")]
        public string CleaningType { get; set; } // Added CleaningType

        [Column("status")]
        public string Status { get; set; } // Added Status

        // Навигационные свойства
        [ForeignKey("RoomNumber")]
        public virtual Room Room { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
    }
}