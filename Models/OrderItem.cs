using System.ComponentModel.DataAnnotations;

public class OrderItem
{   
    [Key]
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public Book? Book { get; set; }
    public int Count { get; set; }
}