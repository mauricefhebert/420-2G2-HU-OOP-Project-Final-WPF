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
        public virtual DbSet<Measurement> Measurements { get; set; } = null!;
        public virtual DbSet<Quantity> Quantities { get; set; } = null!;
        public virtual DbSet<Recipe> Recipes { get; set; } = null!;
        public virtual DbSet<ShoppingList> ShoppingLists { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["FoodManagerConnection"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ingrediant>(entity =>
            {
                entity.ToTable("Ingrediant");

                entity.Property(e => e.IngrediantId).HasColumnName("IngrediantID");

                entity.Property(e => e.IngrediantName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ListItem>(entity =>
            {
                entity.ToTable("List_Item");

                entity.HasIndex(e => e.ShoppingListId, "idx_ShoppingListID");

                entity.Property(e => e.ListItemId).HasColumnName("ListItemID");

                entity.Property(e => e.ShoppingListId).HasColumnName("ShoppingListID");

                entity.HasOne(d => d.ShoppingList)
                    .WithMany(p => p.ListItems)
                    .HasForeignKey(d => d.ShoppingListId)
                    .HasConstraintName("FK__List_Item__Shopp__3E52440B");
            });

            modelBuilder.Entity<Measurement>(entity =>
            {
                entity.ToTable("Measurement");

                entity.HasIndex(e => e.MeasurementUnit, "UQ__Measurem__D3AE278AA895B83F")
                    .IsUnique();

                entity.Property(e => e.MeasurementId).HasColumnName("MeasurementID");
            });

            modelBuilder.Entity<Quantity>(entity =>
            {
                entity.ToTable("Quantity");

                entity.HasIndex(e => e.IngrediantId, "idx_IngrediantID");

                entity.HasIndex(e => e.MeasurementId, "idx_MeasurementID");

                entity.HasIndex(e => e.RecipeId, "idx_RecipeID");

                entity.Property(e => e.QuantityId).HasColumnName("QuantityID");

                entity.Property(e => e.IngrediantId).HasColumnName("IngrediantID");

                entity.Property(e => e.MeasurementId).HasColumnName("MeasurementID");

                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.HasOne(d => d.Ingrediant)
                    .WithMany(p => p.Quantities)
                    .HasForeignKey(d => d.IngrediantId)
                    .HasConstraintName("FK__Quantity__Ingred__49C3F6B7");

                entity.HasOne(d => d.Measurement)
                    .WithMany(p => p.Quantities)
                    .HasForeignKey(d => d.MeasurementId)
                    .HasConstraintName("FK__Quantity__Measur__4AB81AF0");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.Quantities)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("FK__Quantity__Recipe__48CFD27E");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.ToTable("Recipe");

                entity.HasIndex(e => e.UserId, "idx_UserID");

                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Recipe__UserID__412EB0B6");
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
                    .HasConstraintName("FK__Shopping___UserI__3B75D760");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UNIQUE_EMAIL")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "UQ__Users__536C85E4057A9774")
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
