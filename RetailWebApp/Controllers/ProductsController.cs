using Microsoft.AspNetCore.Mvc;
using RetailWebApp.Models;
using RetailWebApp.Services;
using System;
using System.Threading.Tasks;

namespace RetailWebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly TableStorageWService _tableStorageService;
        private readonly BlobService _blobService;

        public ProductsController(TableStorageWService tableStorageService, BlobService blobService)
        {
            _tableStorageService = tableStorageService;
            _blobService = blobService;
        }

        // GET: Products list
        public async Task<IActionResult> Index(string searchString)
        {
            var products = await _tableStorageService.GetAllProducts();

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products
                    .Where(p => p.ProductName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            ViewBag.SearchString = searchString;
            return View(products);
        }

        
        // POST: Add Product form submission
        [HttpPost]
        public async Task<IActionResult> AddProduct(Products product, IFormFile file)
        {
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                var imageUrl = await _blobService.UploadsAsync(stream, file.FileName);
                product.ProductImage = imageUrl;
            }

            if (true)
            {
                product.PartitionKey = "ProductPartition";
                product.RowKey = Guid.NewGuid().ToString();
                
                await _tableStorageService.AddProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }
        
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }
    }
}