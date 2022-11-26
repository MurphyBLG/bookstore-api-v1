using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Int64 OrderId { get; set; }

    public User? User { get; set; }

    public Book? Book { get; set; } 

    public int Count { get; set; }
}