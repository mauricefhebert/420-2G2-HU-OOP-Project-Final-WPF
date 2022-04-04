using System;
using System.Collections.Generic;

namespace ProjectFinal2195109
{
    public partial class ShoppingList
    {
        public ShoppingList()
        {
            ListItems = new HashSet<ListItem>();
        }

        public int ShoppingListId { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<ListItem> ListItems { get; set; }
    }
}
