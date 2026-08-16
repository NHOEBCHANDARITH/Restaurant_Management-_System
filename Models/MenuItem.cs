
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management__System.Models
{
    public class MenuItem
    {
        [Key]
        public int ItemId { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Category Category { get; set; }
    }
}
