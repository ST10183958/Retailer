using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;


namespace RetailWebApp.Models;


public class orders: ITableEntity
{
    [Key]
    public int OrderId { get; set; }
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    
    public ETag ETag { get; set; }
    
    [Required(ErrorMessage = "Please select a customer")]
    public int ID { get; set; }
    [Required(ErrorMessage = "Please select a product")]
    public int ProductID { get; set; }
    
    [Required(ErrorMessage = "Please select a Date")]
    public DateTime Date { get; set; }
    
    [Required(ErrorMessage = "Please select a quantity")]
    public int Quantity { get; set; }
    
}