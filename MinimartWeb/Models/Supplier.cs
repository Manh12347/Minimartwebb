using MinimartWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimartWeb.Model
{
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; }

        [Required, MaxLength(255)]
        public string SupplierName { get; set; }

        [Required, StringLength(10)]
        public string SupplierPhoneNumber { get; set; }

        [Required, MaxLength(255)]
        public string SupplierAddress { get; set; }

        [Required, MaxLength(255)]
        public string SupplierEmail { get; set; }

        public ICollection<ProductType> ProductTypes { get; set; } = new List<ProductType>();
    }
}
