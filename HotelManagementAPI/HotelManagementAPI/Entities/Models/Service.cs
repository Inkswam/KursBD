using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagementAPI.Entities.Models;

[Table("services", Schema = "public")]
public class Service
{
    [Key]
    [Required]
    [Column("id")]
    public required Guid Id { get; set; }
    
    [Required]
    [Column("name")]
    [MaxLength(100)]
    public required string Name { get; set; }
    
    [Column("description")]
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Column("price")]
    public long Price { get; set; }
}