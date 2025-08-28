using Azure;
using Azure.Data.Tables;
using RetailWebApp.Models;

namespace RetailWebApp.Services;

public class TableStorageWService
{
    public readonly TableClient _CustomerTableClient;
    public readonly TableClient _ProductTableClient;
    public readonly TableClient _OrderTableClient;

    public TableStorageWService(string connectionString)
    {
        _CustomerTableClient = new TableClient(connectionString, "Customer");
        _ProductTableClient = new TableClient(connectionString, "Products");
        _OrderTableClient = new TableClient(connectionString, "orders");
    }

    //loop through table customer
    public async Task<List<Customer>> GetAllCustomers()
    {
        var customers = new List<Customer>();

        await foreach (var customer in _CustomerTableClient.QueryAsync<Customer>())
        {
            customers.Add(customer);
        }
        
        return customers;
    }
    //loop through table products
    public async Task<List<Products>> GetAllProducts()
    {
        var products = new List<Products>();

        await foreach (var product in _ProductTableClient.QueryAsync<Products>())
        {
            products.Add(product);
        }
        
        return products;
    }
    
    //add customer
    public async Task AddCustomer(Customer customer)
    {
        if (string.IsNullOrEmpty(customer.PartitionKey) || string.IsNullOrEmpty(customer.RowKey))
        {
            throw new ArgumentException("Invalid partition/row key");
        }

        try
        {
            await _CustomerTableClient.AddEntityAsync(customer);
        }
        catch (RequestFailedException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }
    //add Product
    public async Task AddProduct(Products products)
    {
        if (string.IsNullOrEmpty(products.PartitionKey) || string.IsNullOrEmpty(products.RowKey))
        {
            throw new ArgumentException("Invalid partition/row key");
        }

        try
        {
            await _CustomerTableClient.AddEntityAsync(products);
        }
        catch (RequestFailedException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }
    
    //deletate customer
    public async Task DeleteCustomer(string partitionKey, string rowKey)
    {
        await _CustomerTableClient.DeleteEntityAsync(partitionKey, rowKey);
    }
    
    //add orders 
    public async Task AddOrder(orders orders)
    {
        if (string.IsNullOrEmpty(orders.PartitionKey) || string.IsNullOrEmpty(orders.RowKey))
        {
            throw new ArgumentException("Invalid partition/row key");
        }

        try
        {
            await _OrderTableClient.AddEntityAsync(orders);
        }
        catch (RequestFailedException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }

    public async Task<List<orders>> GetAllOrders()
    {
        var orders = new List<orders>();
        await foreach (var order in _OrderTableClient.QueryAsync<orders>())
        {
            orders.Add(order);
        }
        return orders;
    }

}