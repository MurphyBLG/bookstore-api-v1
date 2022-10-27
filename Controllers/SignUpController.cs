using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

[Route("api/[controller]")]
public class SignUpController : Controller
{
    private readonly BooksDbContext _context;
    public SignUpController(BooksDbContext context)
    {
        this._context = context;
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] UserDTO userDTO)
    {
        var user = from u in _context.Users
                   where u.Username == userDTO.Username
                   select u;

        if (user.Any())
        {
            return BadRequest("User with this username already exists");
        }

        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

        var pbkdf2 = new Rfc2898DeriveBytes(userDTO.Password!, salt, 100000);
        byte[] hash = pbkdf2.GetBytes(20);

        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        var newUser = new User
        {
            PasswordHash = hashBytes,
            Role = "ocherednyara",
            Username = userDTO.Username
        };

        _context.Users!.Add(newUser);
        await _context.SaveChangesAsync();

        return Ok();
    }
}