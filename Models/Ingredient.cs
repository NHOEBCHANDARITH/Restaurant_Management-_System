using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management__System.Models
{
    public class Ingredient
    {
        [Key]
        public int IngredientId { get; set; }

        public int SupplierId { get; set; }

        public string IngredientName { get; set; }

        public int Stock { get; set; }

        public string Unit { get; set; }

        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }
    }
}
