using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace Product.Registry.Admin.Models;

public class ProductModel {
    public int Id { get; set; }

    [Required]
    [Display(Name = "PRODUCT NAME")]
    public string Name { get; set; } =  null!;
    [Required]
    public string Description { get; set; } = null!;

    [DataType(DataType.Currency)]
    [Range(0.01,1000.00,ErrorMessage = "Value for {0} {1:C} and {2:C}")] 
     public decimal Price {get;set;}

     public bool IsActive {get;set;}

    public int CategoryId { get; set; }

    [Display(Name = "Category")]
    public string? CategoryName { get; set; }

    public List<SelectListItem> AvailableCategories { get; set; } = new();
    public static ProductModel FromProduct(Data.Product product) {
        return new ProductModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            IsActive = product.IsActive,
            Price = product.Price,
            CategoryId = product.CategoryId ?? 0,
            CategoryName = product.Category?.Name
        };
    }

    public Data.Product ToProduct() {
        return new Data.Product
        {
            Price = Price,
            Name = Name,
            Description = Description,
            IsActive = IsActive,
            Id = Id,
            CategoryId = CategoryId,
        };
    }
}