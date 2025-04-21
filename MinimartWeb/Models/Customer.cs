using MinimartWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace MinimartWeb.Model
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required, MaxLength(255)]
        public string FirstName { get; set; }

        [Required, MaxLength(255)]
        public string LastName { get; set; }

        [Required, MaxLength(255)]
        public string Email { get; set; }

        [Required, StringLength(10)]
        public string PhoneNumber { get; set; }

        [MaxLength(512)]
        public string? ImagePath { get; set; }

        [Required, MaxLength(255)]
        public string Username { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] Salt { get; set; }

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}
