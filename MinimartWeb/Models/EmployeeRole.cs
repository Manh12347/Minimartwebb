using MinimartWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace MinimartWeb.Model
{
    public class EmployeeRole
    {
        [Key]
        public int RoleID { get; set; }

        [Required, MaxLength(255)]
        public string RoleName { get; set; }

        public string? RoleDescription { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
