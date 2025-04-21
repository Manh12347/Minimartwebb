using MinimartWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace MinimartWeb.Model
{
    public class PaymentMethod
    {
        [Key]
        public int PaymentMethodID { get; set; }

        [Required, MaxLength(50)]
        public string MethodName { get; set; }

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}
