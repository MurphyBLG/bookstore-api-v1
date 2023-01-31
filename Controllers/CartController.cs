using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[Route("api/[controller]")]
public class CartController : Controller
{
    private readonly BooksDbContext _context;
    public CartController(BooksDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult GetCart()
    {
        var user = GetCurrentUser();

        if (user == null)
        {
            return NotFound("User not found");
        }

        var query = _context.Carts!.Include(b => b.Book).Where(u => u.User == user);
        bool cartExists = query.Any();
        if (cartExists)
        {
            return Ok(query.Select(p => new {p.Book, p.Count}));
        } 
        else
        {
            return NotFound("Cart not found");
        }
    }

    [HttpPost]
    public async Task<ActionResult> AddBookToCart([FromBody] Book book)
    {
        var user = GetCurrentUser();

        if (user == null) 
        {
            return BadRequest("User not found");
        } 

        var query = _context.Carts!.Where(u => u.User == user && u.Book!.BookId == book.BookId);
        bool bookInCart = query.Any();
        if (bookInCart)
        {
            query.First().Count++;
        }
        else
        {
            var bookRef = _context.Books!.Where(b => b.BookId == book.BookId).First();
            _context.Carts!.Add(new Cart
            {
                User = user,
                Book = bookRef,
                Count = 1
            });
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