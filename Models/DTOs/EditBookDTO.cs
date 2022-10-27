using System.ComponentModel.DataAnnotations;

public class EditBookDTO
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Author { get; set; }
    [Required]
    [Range(1, 10000000)]
    public decimal Price { get; set; }
}