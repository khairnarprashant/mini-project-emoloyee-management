using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mini_project_emoloyee_management.Data;
using mini_project_emoloyee_management.Models;

namespace mini_project_emoloyee_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeparmentController : ControllerBase
    {
        public readonly ApplicationDbContext _DbConext;
        public DeparmentController(ApplicationDbContext dbContext)
        {
            this._DbConext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var det = await _DbConext.Departments.ToListAsync();
            return Ok(det);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            var dep = new Department()
            {
                DeptId = department.DeptId,
                DeptName = department.DeptName
            };
            await _DbConext.Departments.AddAsync(dep);
            await _DbConext.SaveChangesAsync();
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public IActionResult GetDepartment(int id)
        {
            var dept = _DbConext.Departments.Find(id);
            if (dept == null)
                return NotFound(); 
            return Ok(dept);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(int id, [FromBody] Department department)
        {
            if (id != department.DeptId)
                return BadRequest();

            _DbConext.Entry(department).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _DbConext.SaveChanges();
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _DbConext.Departments.FindAsync(id);
            if (department == null)
                return NotFound();

            _DbConext.Departments.Remove(department);
            await _DbConext.SaveChangesAsync();
            return NoContent();
        }
    }
}

    