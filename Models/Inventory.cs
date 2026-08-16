using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management__System.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        public int IngredientId { get; set; }

        public int Quantity { get; set; }

        public DateTime LastUpdated { get; set; }

        [ForeignKey("IngredientId")]
        public Ingredient Ingredient { get; set; }
    }
}
