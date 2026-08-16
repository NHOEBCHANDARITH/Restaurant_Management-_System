using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant_Management__System.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int CustomerId { get; set; }
        public int TableId { get; set; }
        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public string Status { get; set; }

        // Customer Relationship
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        // Dining Table Relationship
        [ForeignKey("TableId")]
        public DiningTable DiningTable { get; set; }

        // User Relationship
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}