using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management__System.Models
{
    public class DiningTable
    {
        [Key]
        public int TableId { get; set; }
        public string TableNumber { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
    }
}
