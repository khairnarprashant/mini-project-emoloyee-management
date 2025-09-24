using Microsoft.EntityFrameworkCore;
using mini_project_emoloyee_management.Models;

namespace mini_project_emoloyee_management.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<Login> Users { get; set; }

    }
}
