using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mini_project_emoloyee_management.Models
{
    public class LeaveRequest
    {
        [Key]
        public int LeaveId { get; set; }

        [ForeignKey("Employee")]
        public int EmpId { get; set; }
        public Employee? Employee { get; set; }
        public string LeaveType { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; } = "Pending";
        public int? ApprovedBy { get; set; }
        public DateTime AppliedOn { get; set; } = DateTime.Now;

    }
}
