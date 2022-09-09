using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Product.Registry.Admin.Models;
using Product.Registry.Admin.Repository;

namespace Product.Registry.Admin.Logic;
public class ProductLogic : IProductLogic
{

    private readonly IProductRepository _repo;

    public ProductLogic(IProductRepository repo) {
        _repo = repo;
    }
    public async Task AddNewProduct(ProductModel productToAdd)
    { 
        var productToSave = productToAdd.ToProduct();
        await _repo.AddProductAsync(productToSave);
    }

    public async Task<List<ProductModel>> GetAllProducts()
    {
        var products = await _repo.GetAllProductsAsync();
        return products.Select(ProductModel.FromProduct).ToList();
    }

    public async Task<ProductModel?> GetProductById(int id)
    {
        var product = await _repo.GetProductByIdAsync(id);
        return product == null ? null : ProductModel.FromProduct(product);
    }

    public async Task RemoveProduct(int id)
    {
        await _repo.RemoveProductAsync(id);
    }

    public async Task UpdateProduct(ProductModel productToUpdate)
    {
        var productToSave = productToUpdate.ToProduct();
        await _repo.UpdateProductAsync(productToSave);
    }

    public async Task<ProductModel> InitializeProductModel()
    {
        return new ProductModel { AvailableCategories = await GetAvailbleCategoriesFromDB() };
    }

    private async Task<List<SelectListItem>> GetAvailbleCategoriesFromDB()
    {
        var categories = await _repo.GetAllCategoriesAsync();
        var returnList = new List<SelectListItem> {  new("None","") };
        var availableCategoryList = categories.Select(category => new SelectListItem(category.Name, category.Id.ToString()));
        returnList.AddRange(availableCategoryList);
        return returnList;
    }

    public async Task GetAvailableCategories(ProductModel productModel)
    {
        productModel.AvailableCategories = await GetAvailbleCategoriesFromDB();
    }
}

