using MinimartWeb.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimartWeb.Model
{
    public class Sale
    {
        [Key]
        public int SaleID { get; set; }

        [Required]
        public DateTime SaleDate { get; set; }

        [ForeignKey("Customer")]
        public int? CustomerID { get; set; }
        public Customer? Customer { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }

        [ForeignKey("PaymentMethod")]
        public int PaymentMethodID { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public string? DeliveryAddress { get; set; }

        public DateTime? DeliveryTime { get; set; }

        public bool IsPickup { get; set; }

        [Required, MaxLength(50)]
        public string OrderStatus { get; set; }

        public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
    }
}
