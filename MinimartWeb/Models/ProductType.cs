using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimartWeb.Model
{
    public class ProductType
    {
        [Key]
        public int ProductTypeID { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Name")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [Required]
        [Display(Name = "Supplier")]
        public int SupplierID { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [StringLength(255)]
        public string? Tags { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue)]
        [Display(Name = "Stock Amount")]
        public decimal StockAmount { get; set; } = 0;

        [Required]
        [Display(Name = "Measurement Unit")]
        public int MeasurementUnitID { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Expiration Duration (Days)")]
        public int? ExpirationDurationDays { get; set; }

        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Required]
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; } = DateTime.Now;

        [StringLength(512)]
        [Display(Name = "Image Path")]
        public string? ImagePath { get; set; }

        // Navigation properties
        public Category Category { get; set; }
        public Supplier Supplier { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
        public ICollection<SaleDetail> SaleDetails { get; set; } = new HashSet<SaleDetail>();
    }
}
