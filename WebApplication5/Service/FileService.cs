using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
namespace WebApplication5.Service
{
    public class FileService
    {
        string url = "https://lab106125241656.blob.core.windows.net/poster/";
        string conn = "DefaultEndpointsProtocol=https;AccountName=lab106125241656;AccountKey=wpKRejqkY3k+oTlzH556t2raC+PVyr/xAoTIlP7FfUKMn3Dr8+OhS/745WD4StkdEh9BIl1jKgHv+AStaS8DBg==;EndpointSuffix=core.windows.net";
        public async Task UploadFile(IFormFile file, string id)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(conn, "poster");
            await blobContainerClient.CreateIfNotExistsAsync();
            string fileFormat = file.FileName.Split(".")[1].ToLower();
            BlobClient blobClient = blobContainerClient.GetBlobClient(id + "." + fileFormat);
            BlobHttpHeaders httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);
        }


        public async Task<string> GetFileName(string id)
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(conn, "poster");
            await blobContainerClient.CreateIfNotExistsAsync();
            BlobClient blobClient = blobContainerClient.GetBlobClient(id + ".png");
            if (blobClient.Exists())
            {
                return url + id + ".png";
            }
            blobClient = blobContainerClient.GetBlobClient(id + ".jpg");
            if (blobClient.Exists())
            {
                return url + id + ".jpg";
            }

            return url + "noimage.png";
        }
    }
}
