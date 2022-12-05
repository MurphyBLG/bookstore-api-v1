using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
            return Ok(user);
        }

        return BadRequest("Wrong username or password");
    }

    private UserInfoDTO? Authenticate(UserDTO userDTO)
    {
        var currentUser = (from u in _context.Users
                           where u.Username == userDTO.Username
                           select u).FirstOrDefault();

        if (currentUser == null)
        {
            return null;
        }

        var userPass = currentUser.PasswordHash;
        var salt = new byte[16];
        Array.Copy(userPass!, 0, salt, 0, 16);

        var pbkdf2 = new Rfc2898DeriveBytes(userDTO.Password!, salt, 100000);
        byte[] hash = pbkdf2.GetBytes(20);

        for (int i = 0; i < 20; i++)
            if (userPass![i + 16] != hash[i])
                return null;


        var userInfo = (from ui in _context.UserInfos
                        where ui.UserId == currentUser.UserId
                        select ui).FirstOrDefault();

        if (userInfo == null)
        {
            return null;
        }

        UserInfoDTO userInfoDTO = new()
        {
            Token = GenerateToken(userInfo),
            Username = userDTO.Username,
            EMail = userInfo.EMail,
            Name = userInfo.Name,
            Surname = userInfo.Surname,
            Role = currentUser.Role
        };
        return userInfoDTO;
    }

    private string GenerateToken(UserInfo userInfo)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)); // ?????
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); // ?????

        var claims = new[]
        {
            new Claim("userId", userInfo.UserId.ToString()),
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
        signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}