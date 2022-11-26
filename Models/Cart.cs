using System.ComponentModel.DataAnnotations.Schema;

public class Cart
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Int64 CartId { get; set; }

    public virtual User? User { get; set; }

    public virtual Book? Book { get; set; } 

    public int Count { get; set; }
}