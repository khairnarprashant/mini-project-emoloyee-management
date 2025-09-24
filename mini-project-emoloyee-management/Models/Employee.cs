using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mini_project_emoloyee_management.Models
{
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [ForeignKey("Department")]
        [Required(ErrorMessage = "Please Select Department")]
        public int DeptId { get; set; }
        public Department? Department { get; set; }

        [MaxLength(50)]
        public string Designation { get; set; }

        public int salary { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(10)]
        public string Phone { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; }


        public DateTime? ResignationDate { get; set; }


    }
}
