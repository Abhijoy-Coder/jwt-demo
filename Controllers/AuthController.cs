using JwtDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;

namespace JwtDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [AllowAnonymous]
    [HttpPost]
    public ActionResult Login([FromBody]UserLogin login)
    {
        var user = Authenticate(login);
        if (user != null)
        {
            var token = GenerateToken(user);
            return Ok(token);
        }

            return NotFound("user not found");
    }

    private UserModel Authenticate(UserLogin login)
    {
        var currentUser = UserConstants.Users.FirstOrDefault(x => x.UserName == login.Username && x.Password == login.Password);
        if(currentUser != null)
        {
            return currentUser;
        }
        return null;
    }

    private string GenerateToken(UserModel user)
    {
        Console.WriteLine("---> -->" + user.UserName + " : " + user.Password + "--> --> " + _config["Jwt:Key"]);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        user.Role = "Admin";
        var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, ""),
                new Claim(ClaimTypes.Role, user.Role)
            };
            
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);    
    }

}