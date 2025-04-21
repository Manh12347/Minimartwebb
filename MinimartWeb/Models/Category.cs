using MinimartWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimartWeb.Model
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required, MaxLength(255)]
        public string CategoryName { get; set; }

        public string? CategoryDescription { get; set; }

        public ICollection<ProductType> ProductTypes { get; set; } = new List<ProductType>();
    }
}
