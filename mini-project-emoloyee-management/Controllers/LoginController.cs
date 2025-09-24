using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using mini_project_emoloyee_management.Data;
using mini_project_emoloyee_management.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace mini_project_emoloyee_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public LoginController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] Login req)
        {
            if (_context.Users.Any(u => u.Username == req.Username))
                return BadRequest(new { message = "User already exists" });

            var user = new Login
            {
                Username = req.Username,
                Password = req.Password, 
                Role = string.IsNullOrEmpty(req.Role) ? "User" : req.Role
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDto req)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == req.Username && u.Password == req.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                message = "Login successful",
                token,
                userId = user.Id,
                username = user.Username,
                role = user.Role
            });
        }

        private string GenerateJwtToken(Login user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}