using System;
using System.Collections.Generic;

namespace ProjectFinal2195109
{
    public partial class Recipe
    {
        public Recipe()
        {
            Quantities = new HashSet<Quantity>();
        }

        public int RecipeId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? Serving { get; set; }
        public int? PrepTime { get; set; }
        public int? CookTime { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Quantity> Quantities { get; set; }
    }
}
