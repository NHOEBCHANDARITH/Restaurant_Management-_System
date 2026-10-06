using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management__System.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        
        [Required]
        public string PaymentMethod { get; set; }
        
        [Required]
        public DateTime PaymentDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Amount { get; set; }

        // NEW FIELDS FOR CAMBODIAN/MULTI-CURRENCY SUPPORT
        [Required]
        [StringLength(3)]
        public string Currency { get; set; } = "USD"; // Default to USD, supports KHR

        [StringLength(100)]
        public string? TransactionReference { get; set; }

        [Required]
        public string PaymentStatus { get; set; }
        
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }
    }
}
