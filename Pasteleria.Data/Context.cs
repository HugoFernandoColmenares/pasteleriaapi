using Pasteleria.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Pasteleria.Data
{
    public class Context : IdentityDbContext<User, IdentityRole, string>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Global Query Filter for Soft Delete
            modelBuilder.Entity<Recipe>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Ingredient>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<InventoryItem>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Document>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<RecipeIngredient>().HasQueryFilter(e => !e.IsDeleted);

            // Seed Roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "7c280f2b-7c7d-4b8c-8f2c-5f2b7c7d4b8c", Name = "Visitor", NormalizedName = "VISITOR" },
                new IdentityRole { Id = "8d39103c-8d8e-5c9d-903d-6g3c8d8e5c9d", Name = "User", NormalizedName = "USER" },
                new IdentityRole { Id = "9e40214d-9e9f-6d0e-a14e-7h4d9e9f6d0e", Name = "Admin", NormalizedName = "ADMIN" }
            );
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    }
}
