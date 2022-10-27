using System.ComponentModel.DataAnnotations;

public class UserDTO
{   
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Password { get; set; }
}