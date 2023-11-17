using System.Security.Claims;
using JwtDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpGet]
    [Route("Admins")]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminEndPoint()
    {
        var currentUser = GetCurrentUser();
        return Ok($"Hi you are an {currentUser.Role}");
    }

    private UserModel GetCurrentUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var userClaims = identity.Claims;
            return new UserModel
            {
                UserName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
            };
        }
        return null;
    }
}
