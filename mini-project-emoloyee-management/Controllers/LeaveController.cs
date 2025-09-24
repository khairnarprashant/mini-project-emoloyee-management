using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mini_project_emoloyee_management.Data;
using mini_project_emoloyee_management.Models;
using System;

namespace mini_project_emoloyee_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LeaveController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("Apply")]
        public async Task<IActionResult> ApplyLeave([FromBody] LeaveRequest leave)
        {
            var employee = await _context.Employees.FindAsync(leave.EmpId);
            if (employee == null)
                return BadRequest("Employee not found.");

            leave.Status = "Pending";
            leave.AppliedOn = DateTime.Now;

            _context.LeaveRequests.Add(leave);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Leave request submitted successfully" });
        }


        [HttpGet("All")]
        public async Task<IActionResult> GetAllLeaves()
        {
            var leaves = await _context.LeaveRequests
                .Include(l => l.Employee) 
                .ToListAsync();

            return Ok(leaves);
        }

        [HttpGet("ByEmployee/{empId}")]
        public async Task<IActionResult> GetLeavesByEmployee(int empId)
        {
            var employee = await _context.Employees.FindAsync(empId);
            if (employee == null)
                return NotFound("Employee not found.");

            var leaves = await _context.LeaveRequests
                .Where(l => l.EmpId == empId)
                .ToListAsync();

            return Ok(leaves);
        }

        [HttpPut("Approve/{leaveId}")]
        public async Task<IActionResult> ApproveOrRejectLeave(int leaveId, [FromBody] string decision)
        {
            var leave = await _context.LeaveRequests
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(l => l.LeaveId == leaveId);

            if (leave == null)
                return NotFound("Leave request not found.");

            // 2️⃣ Validate decision
            if (decision != "Approved" && decision != "Rejected")
                return BadRequest("Decision must be either 'Approved' or 'Rejected'.");

            // 3️⃣ Get logged-in manager/HR ID
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized("You must be logged in to approve/reject leave.");

            int approverId = int.Parse(userIdClaim);

            // 4️⃣ Update leave entity
            leave.Status = decision;
            leave.ApprovedBy = approverId;

            // 5️⃣ Attach the entity to context and mark as modified
            _context.LeaveRequests.Attach(leave);
            _context.Entry(leave).Property(l => l.Status).IsModified = true;
            _context.Entry(leave).Property(l => l.ApprovedBy).IsModified = true;

            // 6️⃣ Save changes
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Leave {decision} successfully." });
        }

    }
}