using Azure.Storage.Blobs;
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

    // Add customer to customer table
    [HttpPost]
    public async Task<IActionResult> AddCustomer(Customer customer)
    {

        customer.PartitionKey = "CustomerPartition";
        customer.RowKey = Guid.NewGuid().ToString();
        customer.ID = customer.RowKey;
        await _tableStorageService.AddCustomer(customer);
        return View(customer);
    }
    [HttpGet]
    public IActionResult AddCustomer()
    {
        return View();
    }

    public async Task<IActionResult> DeleteCustomer(string PartitionKey, string RowKey)
    {
        await _tableStorageService.DeleteCustomer(PartitionKey, RowKey);
        return RedirectToAction("Index");
    }


}