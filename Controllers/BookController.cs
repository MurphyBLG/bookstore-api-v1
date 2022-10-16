using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
public class BookController : Controller
{
    private readonly BooksDbContext? _context;

    public BookController(BooksDbContext context)
    {
        this._context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Book>> GetBooks()
    {
        return await _context!.Books!.ToListAsync(); // ???
    }

    [HttpPost]
    public async Task<ActionResult> AddBook(AddBookDTO addBookDTO)
    {
        try 
        {
            _context!.Add(new Book
            {
                Name = addBookDTO.Name,
                Author = addBookDTO.Author,
                Price = addBookDTO.Price
            });

            await _context.SaveChangesAsync();

            return Ok();
        }
        catch
        {
            return StatusCode(500);
        }
    }
}