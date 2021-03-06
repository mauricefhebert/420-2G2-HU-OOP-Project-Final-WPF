using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProjectFinal2195109
{
    public partial class FoodManagerDbContext : DbContext
    {
        public FoodManagerDbContext()
        {
        }

        public FoodManagerDbContext(DbContextOptions<FoodManagerDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ingrediant> Ingrediants { get; set; } = null!;
        public virtual DbSet<ListItem> ListItems { get; set; } = null!;
        public virtual DbSet<Recipe> Recipes { get; set; } = null!;
        public virtual DbSet<ShoppingList> ShoppingLists { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["FoodManagerConnection"].ConnectionString);
                //optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["FoodManagerConnectionSchool"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ingrediant>(entity =>
            {
                entity.ToTable("Ingrediant");

                entity.HasIndex(e => e.RecipeId, "idx_RecipeID");

                entity.Property(e => e.IngrediantId).HasColumnName("IngrediantID");

                entity.Property(e => e.IngrediantMeasurementUnit)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IngrediantName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.Ingrediants)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Ingredian__Recip__3E52440B");
            });

            modelBuilder.Entity<ListItem>(entity =>
            {
                entity.ToTable("List_Item");

                entity.HasIndex(e => e.IngrediantId, "idx_IngrediantID");

                entity.HasIndex(e => e.RecipeId, "idx_RecipeID");

                entity.HasIndex(e => e.ShoppingListId, "idx_ShoppingListID");

                entity.Property(e => e.ListItemId).HasColumnName("ListItemID");

                entity.Property(e => e.IngrediantId).HasColumnName("IngrediantID");

                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.Property(e => e.ShoppingListId).HasColumnName("ShoppingListID");

                entity.HasOne(d => d.Ingrediant)
                    .WithMany(p => p.ListItems)
                    .HasForeignKey(d => d.IngrediantId)
                    .HasConstraintName("FK__List_Item__Ingre__4D94879B");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.ListItems)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("FK__List_Item__Recip__4E88ABD4");

                entity.HasOne(d => d.ShoppingList)
                    .WithMany(p => p.ListItems)
                    .HasForeignKey(d => d.ShoppingListId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__List_Item__Shopp__4CA06362");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.ToTable("Recipe");

                entity.HasIndex(e => e.UserId, "idx_UserID");

                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Recipe__UserID__3B75D760");
            });

            modelBuilder.Entity<ShoppingList>(entity =>
            {
                entity.ToTable("Shopping_List");

                entity.HasIndex(e => e.UserId, "idx_UserID");

                entity.Property(e => e.ShoppingListId).HasColumnName("ShoppingListID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ShoppingLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Shopping___UserI__412EB0B6");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username, "UQ__Users__536C85E470CA55D1")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__Users__A9D10534D0289D65")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "idx_UserEmail");

                entity.HasIndex(e => e.Username, "idx_UserName");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
