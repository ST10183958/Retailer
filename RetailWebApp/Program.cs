using RetailWebApp.Models;
using RetailWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var configuration = builder.Configuration;

//Register tablestorage with configuration
builder.Services.AddSingleton(new TableStorageWService(configuration.GetConnectionString("AzureStorage")));

builder.Services.AddSingleton(new BlobService(configuration.GetConnectionString("AzureStorage")));
//Register QueueService with configuration
builder.Services.AddSingleton<QueueService>(sp =>
{
    var connectionString = configuration.GetConnectionString("AzureStorage");
    return new QueueService(connectionString, "orders");
});

//Register file share 
builder.Services.AddSingleton<AzureFileShareService>(sp =>
{
    var connectionString = configuration.GetConnectionString("AzureStorage");
    return new AzureFileShareService(connectionString, "ordershare");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();