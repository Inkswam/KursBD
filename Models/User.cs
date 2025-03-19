using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementSystem.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("user_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        
        [Column("username")]
        [StringLength(50)]
        public string Username { get; set; }
        
        [Column("password_hash")]
        [StringLength(255)]
        public string PasswordHash { get; set; }
        
        [Column("role")]
        [StringLength(20)]
        public string Role { get; set; } // "Admin", "Manager"
    }
}