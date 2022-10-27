using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[Route("api/[controller]")]
public class LoginController : Controller
{
    private IConfiguration _config;
    private readonly BooksDbContext _context;

    public LoginController(IConfiguration config, BooksDbContext context)
    {
        this._config = config;
        this._context = context;
    }

    [AllowAnonymous]
    [HttpPost]
    public ActionResult Login([FromBody] UserDTO userDTO)
    {
        var user = Authenticate(userDTO);

        if (user != null)
        {
            var token = GenerateToken(user.Value!);
            return Ok(token);
        }

        return NotFound("User not found");
    }

    private string GenerateToken(UserInfo userInfo)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])); // ?????
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); // ?????

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userInfo.User!.Username!),
            new Claim(ClaimTypes.Email, userInfo.EMail!),
            new Claim(ClaimTypes.Name, userInfo.Name!),
            new Claim(ClaimTypes.Surname, userInfo.Surname!),
            new Claim(ClaimTypes.Role, userInfo.User!.Role!)
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
        _config["Jwt:Audience"],
        claims,
        expires: DateTime.Now.AddMinutes(15),
        signingCredentials: credentials); // ???

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private ActionResult<UserInfo> Authenticate(UserDTO userDTO)
    {
        var currentUser = from u in _context.Users
                          where u.Username == userDTO.Username
                          select u;
        
        if (!currentUser.Any()) 
        {
            return NotFound("User not found");
        }

        // password check
        throw new NotImplementedException();
    }
}