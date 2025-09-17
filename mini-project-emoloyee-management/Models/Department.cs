using System.ComponentModel.DataAnnotations;

namespace mini_project_emoloyee_management.Models
{
    public class Department
    {
        [Key]
        public int DeptId { get; set; }

        [Required]
        [MaxLength(100)]
        public string DeptName { get; set; }
    }
}
