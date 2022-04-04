using System;
using System.Collections.Generic;

namespace ProjectFinal2195109
{
    public partial class Ingrediant
    {
        public Ingrediant()
        {
            Quantities = new HashSet<Quantity>();
        }

        public int IngrediantId { get; set; }
        public string IngrediantName { get; set; } = null!;

        public virtual ICollection<Quantity> Quantities { get; set; }
    }
}
