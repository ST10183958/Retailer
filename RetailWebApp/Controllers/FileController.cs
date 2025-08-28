using Microsoft.AspNetCore.Mvc;
using RetailWebApp.Models;

namespace RetailWebApp.Controllers;

public class FileController : Controller
{
    private readonly AzureFileShareService _fileShareService;

    public FileController(AzureFileShareService fileShareService)
    {
        _fileShareService = fileShareService;
    }

    public async Task<IActionResult> Index()
    {
        List<FileModel> files;
        try
        {
            files = await _fileShareService.ListFilesAsync("uploads");
        }
        catch (Exception ex)
        {
            ViewBag.Message = $"Failed to load files :{ex.Message}";
            files = new List<FileModel>();
        }
        return View(files);
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError(string.Empty, "Please upload a file");
            return await Index();
        }

        try
        {
            using (var stream = file.OpenReadStream())
            {
                string directoryName = "uploads";
                string fileName = file.FileName;
                await _fileShareService.UpLoadFileAsync(directoryName, fileName, stream);
            }

            TempData["Message"] = $"File '{file.FileName}' uploaded successfully'";
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"Failed to upload file :{ex.Message}";
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> DownloadFile(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return BadRequest("Please upload a file");
        }

        try
        {
            var fileStream = await _fileShareService.DownLoadFileAsync("uploads", fileName);
            if (fileStream == null)
            {
                return NotFound($"File {fileName}' not found");
            }

            return File(fileStream, "application/octet-stream", fileName);
        }
        catch (Exception e)
        {
            return BadRequest($"Error Downloading File: {e.Message}");
        }
    }
}