using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
public class CartController : Controller
{
    private readonly BooksDbContext _context;
    public CartController(BooksDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetCart()
    {
        var user = GetCurrentUser();

        if (user == null)
        {
            return BadRequest("User not found");
        }

        var query = _context.Carts!.Include(b => b.Book).Where(u => u.User == user);
        bool cartExists = query.Any();
        if (cartExists)
        {
            return Ok(query);
        } 
        else
        {
            return BadRequest("Cart not found");
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> AddBookToCart([FromBody] Book book)
    {
        var user = GetCurrentUser();

        // user not found error

        var query = _context.Carts!.Where(u => u.User == user && u.Book!.BookId == book.BookId);
        bool bookInCart = query.Any();
        if (bookInCart)
        {
            query.First().Count++;
        }
        else
        {
            var bookRef = _context.Books.Where(b => b.BookId == book.BookId).First();
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

    private User GetCurrentUser()
    {
        var userId = Int32.Parse(User.FindFirstValue("userId"));

        // USER NOT FOUND ERROR

        var user = _context.Users!.Where(u => u.UserId == userId).First();

        return user;
    }
}