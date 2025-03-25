using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelManagementAPI.Entities.Enums;

namespace HotelManagementAPI.Entities.Models;

[Table("users", Schema = "public")]
public class User
{
    [Key]
    [Required]
    [Column("email")]
    [MaxLength(254)]
    public required string Email { get; set; }
    
    [Required]
    [Column("first_name")]
    [MaxLength(50)]
    public required string FirstName { get; set; }
    
    [Required]
    [Column("last_name")]
    [MaxLength(50)]
    public required string LastName { get; set; }
    
    [Required]
    [Column("phone_number")]
    [MaxLength(13)]
    [MinLength(13)]
    public required string PhoneNumber { get; set; }
    
    [Required]
    [Column("birth_date")]
    public required DateOnly BirthDate { get; set; }
    
    [Required]
    [Column("password_salt")]
    [MaxLength(64)]
    public required string PasswordSalt { get; set; }
    
    [Required]
    [Column("password_hash")]
    [MaxLength(128)]
    public required string PasswordHash { get; set; }
    
    [Required]
    [Column("user_role")]
    public required EUserRole UserRole { get; set; }
    
    public Administrator? Administrator { get; set; }
    public Receptionist? Receptionist { get; set; }
    public Guest? Guest { get; set; }
    
}