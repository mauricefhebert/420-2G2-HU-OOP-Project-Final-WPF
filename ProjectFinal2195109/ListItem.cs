using System;
using System.Collections.Generic;

namespace ProjectFinal2195109
{
    public partial class ListItem
    {
        public int ListItemId { get; set; }
        public int? ShoppingListId { get; set; }

        public virtual ShoppingList? ShoppingList { get; set; }
    }
}
