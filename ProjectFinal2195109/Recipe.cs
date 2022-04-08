using System;
using System.Collections.Generic;

namespace ProjectFinal2195109
{
    public partial class Recipe
    {
        public Recipe()
        {
            Ingrediants = new HashSet<Ingrediant>();
            ListItems = new HashSet<ListItem>();
        }

        public int RecipeId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public double? Serving { get; set; }
        public bool? IsActive { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Ingrediant> Ingrediants { get; set; }
        public virtual ICollection<ListItem> ListItems { get; set; }
    }
}
