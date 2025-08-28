using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;

namespace RetailWebApp.Models;

public class Products : ITableEntity
{
    public string? ProductID { get; set; }
    public string? ProductName { get; set; }
    public string? ProductImage { get; set; }
    public string? Price { get; set; }
    public string? Quantity { get; set; }
    
    // ITableEntity implementation
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public ETag ETag { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
}