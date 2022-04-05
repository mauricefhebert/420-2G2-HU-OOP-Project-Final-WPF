using System;
using System.Collections.Generic;

namespace ProjectFinal2195109
{
    public partial class Ingrediant
    {
        public Ingrediant()
        {
            ListItems = new HashSet<ListItem>();
        }

        public int IngrediantId { get; set; }
        public string IngrediantName { get; set; } = null!;
        public double IngrediantQuantity { get; set; }
        public string? IngrediantMeasurementUnit { get; set; }
        public int? RecipeId { get; set; }

        public virtual Recipe? Recipe { get; set; }
        public virtual ICollection<ListItem> ListItems { get; set; }
    }
}
