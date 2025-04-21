using MinimartWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimartWeb.Model
{
    public class ProductType
    {
        [Key]
        public int ProductTypeID { get; set; }

        [Required, MaxLength(255)]
        public string ProductName { get; set; }

        [Required]
        public string ProductDescription { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? Tags { get; set; }

        [Required]
        public decimal StockAmount { get; set; }

        public int? ExpirationDurationDays { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [MaxLength(512)]
        public string? ImagePath { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        [ForeignKey("Supplier")]
        public int SupplierID { get; set; }
        public Supplier Supplier { get; set; }

        [ForeignKey("MeasurementUnit")]
        public int MeasurementUnitID { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }

        public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
    }
}
