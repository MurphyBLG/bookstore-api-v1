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
    public async Task<ActionResult> AddBook([FromBody] AddBookDTO addBookDTO)
    {
        try
        {
            var book = new Book
            {
                Name = addBookDTO.Name,
                Author = addBookDTO.Author,
                Price = addBookDTO.Price
            };

            _context!.Add(book);
            await _context.SaveChangesAsync();

            return Ok(book);
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpDelete("{bookId:int}")]
    public async Task<ActionResult> DeleteBook(int bookId)
    {
        var book = _context!.Books!.Where(b => b.BookId == bookId).First();

        if (book == null)
        {
            return NotFound();
        }

        _context.Remove(book);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{bookId:int}")]
    public async Task<ActionResult> UpdateBook(int bookId, [FromBody] EditBookDTO editBookDTO) {
        var book = _context!.Books!.Where(b => b.BookId == bookId).First();

        if (book == null)
        {
            return NotFound();
        }
        
        _context.Entry(book).CurrentValues.SetValues(editBookDTO);

        await _context.SaveChangesAsync();
        return Ok();
    }
}