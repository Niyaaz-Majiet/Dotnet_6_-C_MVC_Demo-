using Microsoft.EntityFrameworkCore;
using Product.Registry.Admin.Data;

namespace Product.Registry.Admin.Repository;

public class ProductRepository : IProductRepository
{

    private readonly ProductContext _context;


    public ProductRepository(ProductContext context) {
        this._context = context;
    }
    public async Task<Data.Product> AddProductAsync(Data.Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;

    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int categoryId)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);  
    }

    public async Task<List<Data.Product>> GetAllProductsAsync()
    {
        return await _context.Products.Include(P => P.Category).ToListAsync();
    }

    public async Task<Data.Product?> GetProductByIdAsync(int productId)
    {
        return await _context.Products.Include(P => P.Category).FirstOrDefaultAsync(m => m.Id == productId);
    }

    public async Task RemoveProductAsync(int productIdToRemove)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productIdToRemove);

        if (product != null) {
          _context.Products.Remove(product);    
          await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateProductAsync(Data.Product product)
    {
        try
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
        }
        catch(DbUpdateConcurrencyException) {
            if (_context.Products.Any(e => e.Id == product.Id)) { 
             throw;
            }
        }
    }
}



