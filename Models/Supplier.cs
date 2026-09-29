using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management__System.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(100)]
        public string SupplierName { get; set; }
        [Required]
        [StringLength(20)]
        public string Phone { get; set; }
        [Required]
        [StringLength(250)]
        public string Address { get; set; }
    }
}
