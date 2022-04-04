using System;
using System.Collections.Generic;

namespace ProjectFinal2195109
{
    public partial class Quantity
    {
        public Quantity()
        {
            ListItems = new HashSet<ListItem>();
        }

        public int QuantityId { get; set; }
        public double QuantityValue { get; set; }
        public int? RecipeId { get; set; }
        public int? IngrediantId { get; set; }
        public int? MeasurementId { get; set; }

        public virtual Ingrediant? Ingrediant { get; set; }
        public virtual Measurement? Measurement { get; set; }
        public virtual Recipe? Recipe { get; set; }
        public virtual ICollection<ListItem> ListItems { get; set; }
    }
}
