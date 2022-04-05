using System;
using System.Collections.Generic;

namespace ProjectFinal2195109
{
    public partial class ListItem
    {
        public int ListItemId { get; set; }
        public int? ShoppingListId { get; set; }
        public int? IngrediantId { get; set; }

        public virtual Ingrediant? Ingrediant { get; set; }
        public virtual ShoppingList? ShoppingList { get; set; }
    }
}
