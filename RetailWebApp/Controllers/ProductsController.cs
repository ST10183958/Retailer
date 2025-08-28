using Microsoft.AspNetCore.Mvc;
using RetailWebApp.Models;
using RetailWebApp.Services;

namespace RetailWebApp.Controllers;

public class ProductsController : Controller
{
    
    private readonly TableStorageWService _tableStorageService;
    private readonly BlobService  _blobService;
    private readonly TableStorageWService _tableStorageWService;

    public ProductsController(TableStorageWService tableStorageService)
    {
        _blobService = _blobService;
        _tableStorageService = tableStorageService;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        var products = await _tableStorageService.GetAllProducts();
        return View(products);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(Products products, IFormFile file)
    {
        if (file != null)
        {
            using var stream = file.OpenReadStream();
            var imageUrl = await _blobService.UploadsAsync(stream, file.FileName);
            products.ProductImage = imageUrl;
        }

        if (ModelState.IsValid)
        {
            products.PartitionKey = "ProductsParition";
            products.RowKey = Guid.NewGuid().ToString();
            await _tableStorageService.AddProduct(products);
            return RedirectToAction("Index");
        }
        return View(products);
    }
    
    [HttpGet]
    public IActionResult AddProduct()
    {
        return View();
    }

}