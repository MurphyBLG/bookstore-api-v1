using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize]
[Route("api/[controller]")]
public class OrderController : Controller
{
    private readonly BooksDbContext _context;

    public OrderController(BooksDbContext context)
    {
        this._context = context;
    }

    [Authorize]
    [HttpGet]
    public ActionResult GetOrders()
    {
        var user = GetCurrentUser();
        if (user == null)
        {
            return BadRequest("User not found");
        }
        
        Dictionary<int, Array> payload = new();
        var orders = _context.Orders!.Where(o => o.User == user).ToList();
        foreach (var order in orders)
        {
            var orderItems = _context.OrderItems!.Where(o => o.OrderId == order.OrderId)
                .Include(b => b.Book)
                .Select(r => new {r.Book, r.Count}).ToArray();
            payload[order.OrderId] = orderItems;
        }

        return Ok(payload);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> MakeOrder()
    {
        var user = GetCurrentUser();
        if (user == null)
        {
            return BadRequest("User not found");
        }

        var itemsFromCart = _context.Carts!.Include(b => b.Book).Where(u => u.User == user).ToList();
        if (itemsFromCart == null)
        {
            return BadRequest("There is no items in cart");
        }

        var currentOrder = new Order 
        {
            User = user
        };

        _context.Orders!.Add(currentOrder);
        await _context.SaveChangesAsync();

        foreach (var item in itemsFromCart)
        {
            _context.OrderItems!.Add(new OrderItem
            {
                OrderId = currentOrder.OrderId,
                Book = item.Book,
                Count = item.Count,
            });

            _context.Carts!.Remove(item);
        }
        await _context.SaveChangesAsync();

        return Ok();
    }

    private User? GetCurrentUser()
    {
        var userId = Int32.Parse(User.FindFirstValue("userId")!);

        if (userId < 0) 
        {
            return null;
        }

        var user = _context.Users!.Where(u => u.UserId == userId).First();

        return user;
    }
}