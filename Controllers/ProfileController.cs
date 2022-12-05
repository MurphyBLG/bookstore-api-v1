using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
public class ProfileController : Controller
{
    private readonly BooksDbContext _context;
    public ProfileController(BooksDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult> ChangeUserInfo([FromBody] UserInfoDTO userInfoDTO)
    {
        var user = GetCurrentUser();

        if (user == null)
        {
            return BadRequest("User not found");
        }

        var userInfo = _context.UserInfos!.Where(u => u.UserId == user.UserId).First();

        if (userInfo != null) {
            userInfo.Name = userInfoDTO.Name;
            userInfo.Surname = userInfoDTO.Surname;
            userInfo.EMail = userInfoDTO.EMail;

            await _context.SaveChangesAsync();
            return Ok();
        }
        
        return BadRequest("User info not found");
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