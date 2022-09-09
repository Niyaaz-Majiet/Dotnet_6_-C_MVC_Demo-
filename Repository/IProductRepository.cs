using Product.Registry.Admin.Data;
namespace Product.Registry.Admin.Repository;

public interface IProductRepository
{
    Task<List<Data.Product>> GetAllProductsAsync();

    Task<Data.Product?> GetProductByIdAsync(int productId);

    Task<Data.Product> AddProductAsync(Data.Product product);

    Task UpdateProductAsync(Data.Product product);

    Task RemoveProductAsync(int productIdToRemove);

    Task<List<Category>> GetAllCategoriesAsync();

    Task<Category?> GetCategoryByIdAsync(int categoryId);
 }
