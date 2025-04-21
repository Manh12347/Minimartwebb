using MinimartWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimartWeb.Model
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        [Required, MaxLength(255)]
        public string FirstName { get; set; }

        [Required, MaxLength(255)]
        public string LastName { get; set; }

        [Required, MaxLength(255)]
        public string Email { get; set; }

        [Required, StringLength(10)]
        public string PhoneNumber { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required, MaxLength(100)]
        public string CitizenID { get; set; }

        public decimal? Salary { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        [MaxLength(512)]
        public string? ImagePath { get; set; }

        [ForeignKey("EmployeeRole")]
        public int RoleID { get; set; }
        public EmployeeRole EmployeeRole { get; set; }

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
        public Admin? Admin { get; set; }
    }
}
