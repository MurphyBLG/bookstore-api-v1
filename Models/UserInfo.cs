using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserInfo
{
    [Key]
    public int UserId { get; set; }
    public User? User { get; set; }

    [Required]
    [Column(TypeName = "varchar(255)")]
    public string? Name { get; set; }
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string? Surname { get; set; }
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string? EMail { get; set; } 
}