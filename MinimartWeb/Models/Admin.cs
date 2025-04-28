using System.ComponentModel.DataAnnotations;

namespace MinimartWeb.Model
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }

        [Required]
        [Display(Name = "Employee ID")]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [MaxLength(64)]
        [Display(Name = "Password Hash")]
        public byte[] PasswordHash { get; set; }

        [MaxLength(64)]
        [Display(Name = "Salt")]
        public byte[] Salt { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Last Login")]
        public DateTime? LastLogin { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        // Navigation property
        public Employee Employee { get; set; }
    }
}
