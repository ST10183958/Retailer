using Microsoft.AspNetCore.Mvc;
using RetailWebApp.Models;
using RetailWebApp.Services;

namespace RetailWebApp.Controllers;

public class CustomerController : Controller
{
    private readonly TableStorageWService _tableStorageService;

    public CustomerController(TableStorageWService tableStorageService)
    {
        _tableStorageService = tableStorageService;
    }
    
    public async Task<IActionResult> Index()
    {
        var customers = await _tableStorageService.GetAllCustomers();
        return View(customers);
    }

    [HttpPost]
    public async Task<IActionResult> AddCustomer(Customer customer)
    {
        customer.PartitionKey = "Customer";
        customer.RowKey = Guid.NewGuid().ToString();
        await _tableStorageService.AddCustomer(customer);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult AddCustomer()
    {
        return View();
    }

}