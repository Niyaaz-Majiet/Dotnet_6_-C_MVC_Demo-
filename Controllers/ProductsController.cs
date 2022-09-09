using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Product.Registry.Admin.Logic;
using Product.Registry.Admin.Models;
using System.Net.Sockets;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Product.Registry.Admin.Controllers;
[Authorize]
public class  ProductsController : Controller{

    private readonly IProductLogic _logic;
    private readonly ILogger<ProductsController> logger;
    public ProductsController(IProductLogic logic, ILogger<ProductsController> logger)
    {
        _logic = logic;
        this.logger = logger;
    }
    public async Task<IActionResult> Index(){
        var products = await _logic.GetAllProducts();
        return View(products);
   }

   public async Task<IActionResult> Details(int id){
        var product = await _logic.GetProductById(id);
        if (product == null)
        {
            logger.LogInformation("Details not found for ID {id}", id);
            return View("NotFound");
        }

        return View(product);
    }

    public async Task<IActionResult> Edit(int? id) {
        if (id == null) {
            logger.LogInformation("No ID passed for edit");
            return View("NotFound");
        };

        var productModal = await _logic.GetProductById(id.Value);
        if (productModal == null) {
            logger.LogInformation("No product was found for {id}", id);
            return View("NotFound");
        }

        await _logic.GetAvailableCategories(productModal);

        return View(productModal);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,ProductModel product) { 
        if(id != product.Id) return View("NotFound");

        if (ModelState.IsValid) { 
            await _logic.UpdateProduct(product);
            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }

    public async Task<IActionResult> Create()
    {
        var model = await _logic.InitializeProductModel();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductModel product)
    {

       // if (!ModelState.IsValid)
      //  {
      //      return View(product);
       //  }

        if (ModelState.IsValid) {
            await _logic.AddNewProduct(product);
            return RedirectToAction(nameof(Index));
        }

        return View(product);

        //try { 
         //await _logic.AddNewProduct(product);
         //return RedirectToAction(nameof(Index));  
        //}
        //catch (ValidationException valEx)
       // {
        //    var results = new ValidationResult(valEx.Errors);
        //    results.AddToModelState(ModelState, null);
        //    return View(product);
        //}

    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return View("NotFound");

        var productModal = await _logic.GetProductById(id.Value);
        if(productModal == null) return View("NotFound");

        return View(productModal);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) { 
        await _logic.RemoveProduct(id);
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { 
           RequestId =  HttpContext.TraceIdentifier
        });
    }
}