using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Book
{
    public int BookId { get; set; }
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string? Name { get; set; }
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string? Author { get; set; }
    [Required]
    [Column(TypeName = "decimal(11, 2)")]
    [Range(1, 10000000)]
    public decimal Price { get; set; }
}