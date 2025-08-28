using Azure.Storage.Blobs;

namespace RetailWebApp.Services;

public class BlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName = "productimages";


    public async Task<string> UploadsAsync(Stream fileStream, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream);
        return blobClient.Uri.ToString();
    }
    
    
}