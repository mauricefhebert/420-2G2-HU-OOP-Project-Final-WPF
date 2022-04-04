using System;
using System.Collections.Generic;

namespace ProjectFinal2195109
{
    public partial class User
    {
        public User()
        {
            Recipes = new HashSet<Recipe>();
            ShoppingLists = new HashSet<ShoppingList>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<ShoppingList> ShoppingLists { get; set; }
    }
}
