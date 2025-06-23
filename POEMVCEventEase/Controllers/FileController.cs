
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLDVPOEMVCEventEase.Controllers
{
    public class FileController : Controller
    {
    
       // Reference Blob Storage
       //According to Reece Waving (2025), this controller  handles file uploads

       //This controller handles image file uploads which helped me understand how to do the blob storage
    
        private readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=st10439514;AccountKey=6TOufnFpuZK5u71Q/aogzsxjQJtjMsc0qQ/kjWLRcdEewjh64SnDrZ64Ibr0gT9I6xFnpXaGbmn3+AStpgLlYQ==;EndpointSuffix=core.windows.net";
        private readonly string containerName = "images";
        public async Task<IActionResult> Index()
        {
            var URLs = await FetchURLsAsync();
            return View(URLs);
        }

        
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile uploadedFile)
        {
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                await UploadFileToBlobStorageAsync(uploadedFile);
            }
            
            return RedirectToAction("Index");
        }

        public IActionResult ViewFile(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
            {
                return NotFound("File not found");
            }
            ViewBag.FileUrl = fileUrl;
            return View();
        }

        private async Task<List<string?>> FetchURLsAsync()
        {
            
            var URLs = new List<string>();
            var containerClient = new BlobContainerClient(connectionString, containerName);

            
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                var blobClient = containerClient.GetBlobClient(blobItem.Name);
                URLs.Add(blobClient.Uri.ToString());
            }

            return URLs;
        }


        private async Task UploadFileToBlobStorageAsync(IFormFile uploadedFile)
        {
            var containerClient = new BlobContainerClient(connectionString, containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            
            var blobClient = containerClient.GetBlobClient(uploadedFile.FileName);
            
            using (var stream = uploadedFile.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }
        }
    }
}

/*References

Reece Waving, 2025. All about the blob storage - CLDV6211. (Version 2.0) [Source Code] https://www.youtube.com/watch?v=tCROBkSoi3Y&list=PL480DYS-b_kevhFsiTpPIB2RzhKPig4iK&index=9&t=281s [Accessed 12 May 2025]

*/