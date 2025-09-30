using Microsoft.AspNetCore.Mvc;
using RetailWebApp.Models;
using RetailWebApp.Services;

namespace RetailWebApp.Controllers;

public class OrdersController : Controller
{
    private readonly TableStorageWService _tableStorageService;
    private readonly QueueService _queueService;

    public OrdersController(TableStorageWService tableStorageService, QueueService queueService)
    {
        _tableStorageService = tableStorageService;
        _queueService = queueService;
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        var orders = await _tableStorageService.GetAllOrders();
        return View(orders);
    }

    public async Task<IActionResult> AddOrder()
    {
        var Customers  = await _tableStorageService.GetAllCustomers();
        var Products = await _tableStorageService.GetAllProducts();

        //Handling null or empty list
        if (Customers == null || Products.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "You must enter at least one customer");
            return View();
        }
        if (Products == null || Products.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "You must enter at least one product");
            return View();
        }
        ViewData["Customers"] = Customers;
        ViewData["Products"] = Products;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder(orders orders)
    {
        if (true)
        {
            //tableService
            orders.Date = DateTime.SpecifyKind(orders.Date, DateTimeKind.Utc);
            orders.PartitionKey = "OrderPartition";
            orders.RowKey = Guid.NewGuid().ToString();
            await _tableStorageService.AddOrder(orders);
            //Message queue
            string message = $"New Order added by User {orders.ID}" + $"for product {orders.ProductID}" + $"Order Date: {orders.Date}";
            await _queueService.SendMessage(message);
            return RedirectToAction("Index");
        }
        //Reload if failed
        var customers = await _tableStorageService.GetAllCustomers();
        var products = await _tableStorageService.GetAllProducts();
        ViewData["Customers"] = customers;
        ViewData["Products"] = products;
        return View(orders);
    }

    
}