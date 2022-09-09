using Microsoft.EntityFrameworkCore;

namespace Product.Registry.Admin.Data;

public class ProductContext : DbContext {

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    public string DbPath { get; set; }

    public ProductContext() {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        DbPath = Path.Join(path, "product.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");

    public void SeedInitialData() {
        if (Products.Any()) {
            Products.RemoveRange(Products);
            SaveChanges();
        }

        if (Categories.Any()) {
            Categories.RemoveRange(Categories);
            SaveChanges(); 
        }

        var footwearCat = new Category { Id = 1000, Name = "Footwear" };
        var equipmentCat = new Category { Id = 2000, Name = "Equipment" };


        Products.Add(new Product { Id = 1, Name = "Trail Blazer", Description = "Test 1", IsActive = true, Price = 69.99M ,Category = footwearCat, CategoryId = 1000});

        Products.Add(new Product { Id = 2, Name = "Coast Liner", Description = "Test 2", IsActive = true, Price = 49.99M, Category = equipmentCat, CategoryId = 2000 });

        Products.Add(new Product { Id = 3, Name = "Woods Man", Description = "Test 3", IsActive = true, Price = 59.99M, Category = footwearCat, CategoryId = 1000 });

        Products.Add(new Product { Id = 4, Name = "Base Camp", Description = "Test 4", IsActive = true, Price = 29.99M, Category = equipmentCat, CategoryId = 2000 });

        Categories.Add(footwearCat);
        Categories.Add(equipmentCat);

        SaveChanges();
    }
    
}