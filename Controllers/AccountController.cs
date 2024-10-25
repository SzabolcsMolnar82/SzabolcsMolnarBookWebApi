using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SzabolcsMolnarBookWebApi.DTOs;
using SzabolcsMolnarBookWebApi.Entities;

namespace SzabolcsMolnarBookWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config) : ControllerBase
    {

        [HttpPost("register")]

        public async Task<IActionResult> Register(PostRegisterDto register)
        {
            var newUser = new ApplicationUser()
            {
                Email = register.EmailAddress,
                PasswordHash = register.Password,
                UserName = register.EmailAddress.Split('@')[0]
            };


            var user = await userManager.FindByEmailAsync(newUser.Email);
            if (user is not null)
            {
                return BadRequest("Ez a felhasználó már regisztrálva van.");
            }


            var createUser = await userManager.CreateAsync(newUser, register.Password);


            var checkAdmin = await roleManager.FindByNameAsync("Admin");
            if (checkAdmin is not null)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                await userManager.AddToRoleAsync(newUser, "Admin");
                return Ok("Admin felhasználó létrehozva.");
            }

            var checkUser = await roleManager.FindByNameAsync("User");
            if (checkUser is null)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = "User" });
            }

            await userManager.AddToRoleAsync(newUser, "User");

            return Ok("Felhasználó létrehozva");
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(PostLoginDto login)
        {
            if (login == null) 
            {
                 return BadRequest("Üres bejelentkezési paraméterek");
            }
            var user = await userManager.FindByEmailAsync(login.Username);

            if (user is null)
            {
                return NotFound("Nincs ilyen felhasználónév");
            }

            bool checkUserPasswords = await userManager.CheckPasswordAsync(user, login.Password);

            if (!checkUserPasswords)
            {
                return BadRequest("Helytelen Belépés.");
            }

            var userRole = await userManager.GetRolesAsync(user);

            string token = GenerateToken(user.Id, user.UserName!, user.Email!, userRole.First());

            return Ok(token);
        }

        private string GenerateToken(string id, string name, string email, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials:credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }



    }

}








/*
public IActionResult Index()
{
    return View();
}
*/