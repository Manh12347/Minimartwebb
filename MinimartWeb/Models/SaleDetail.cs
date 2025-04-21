using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimartWeb.Model
{
    public class SaleDetail
    {
        [Key]
        public int SaleDetailID { get; set; }

        [ForeignKey("Sale")]
        public int SaleID { get; set; }
        public Sale Sale { get; set; }

        [ForeignKey("ProductType")]
        public int ProductTypeID { get; set; }
        public ProductType ProductType { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public decimal ProductPriceAtPurchase { get; set; }
    }
}
