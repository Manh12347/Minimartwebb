using MinimartWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace MinimartWeb.Model
{
    public class MeasurementUnit
    {
        [Key]
        public int MeasurementUnitID { get; set; }

        [Required, MaxLength(50)]
        public string UnitName { get; set; }

        public string? UnitDescription { get; set; }

        [Required]
        public bool IsContinuous { get; set; }

        public ICollection<ProductType> ProductTypes { get; set; } = new List<ProductType>();
    }
}
