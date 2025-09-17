using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mini_project_emoloyee_management.Data;
using mini_project_emoloyee_management.Models;

namespace mini_project_emoloyee_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _DbContext;

        public EmployeeController(ApplicationDbContext context)
        {
            _DbContext = context;
        }
        //nb

        [HttpPost]  //sdsd
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            try
            {
                await _DbContext.Employees.AddAsync(employee);
                await _DbContext.SaveChangesAsync();
                var empWithDept = await _DbContext.Employees
                    .Include(e => e.Department)
                    .FirstOrDefaultAsync(e => e.EmpId == employee.EmpId);

                return Ok(empWithDept);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _DbContext.Employees
                .Include(e => e.Department)  
                .ToListAsync();

            return Ok(employees);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _DbContext.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmpId == id);

            if (employee == null)
                return NotFound("Employee not found");

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee updatedEmployee)
        {
            if (id != updatedEmployee.EmpId)
                return BadRequest("Employee ID mismatch");

            var existingEmp = await _DbContext.Employees.FindAsync(id);

            if (existingEmp == null)
                return NotFound("Employee not found");

            existingEmp.Name = updatedEmployee.Name;
            existingEmp.DeptId = updatedEmployee.DeptId;
            existingEmp.Designation = updatedEmployee.Designation;
            existingEmp.Email = updatedEmployee.Email;
            existingEmp.Phone = updatedEmployee.Phone;
            existingEmp.Address = updatedEmployee.Address;
            existingEmp.JoiningDate = updatedEmployee.JoiningDate;
            existingEmp.ResignationDate  = updatedEmployee.ResignationDate;

            await _DbContext.SaveChangesAsync();

            var empWithDept = await _DbContext.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmpId == id);

            return Ok(empWithDept);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _DbContext.Employees.FindAsync(id);

            if (employee == null)
                return NotFound("Employee not found");

            _DbContext.Employees.Remove(employee);
            await _DbContext.SaveChangesAsync();

            return Ok(new { message = "Employee deleted successfully" });
        }
    }
}

  