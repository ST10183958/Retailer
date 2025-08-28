using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;

namespace RetailWebApp.Models;

public class Customer : ITableEntity
{
    [Key]
    public string? ID { get; set; }
    public string? CustomerFirstName { get; set; }
    public string? CustomerLastName { get; set; }
    public string? CustomerEmail { get; set; }
    
    // ITableEntity implementation
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public ETag ETag { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    
}