using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management__System.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
