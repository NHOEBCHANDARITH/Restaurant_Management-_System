using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management__System.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }
    }
}
