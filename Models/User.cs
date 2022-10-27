using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    public int UserId { get; set; }
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string? Username { get; set; }
    [Required]
    [Column(TypeName = "bytea")]
    public byte[]? PasswordHash { get; set; }
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string? Role { get; set; }

    public UserInfo? userInfo { get; set; }
}