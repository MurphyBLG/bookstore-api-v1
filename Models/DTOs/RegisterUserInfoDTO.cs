using System.ComponentModel.DataAnnotations;

public class RegisterUserDTO
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Surname { get; set; }
    [Required]
    public string? EMail { get; set; } 
}