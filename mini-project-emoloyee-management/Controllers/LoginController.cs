using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using mini_project_emoloyee_management.Data;
using mini_project_emoloyee_management.Models;
using System;

namespace mini_project_emoloyee_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Login([FromBody] Login req)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == req.Username && u.Password == req.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(new { message = "Login successful", userId = user.Id, username = user.Username });
        }
    }
}

