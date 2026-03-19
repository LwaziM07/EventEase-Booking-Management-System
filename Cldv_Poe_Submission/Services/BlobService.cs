using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Cldv_Poe_Submission.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient blobServiceClient;

        private const string containerName = "images"; //Creating readonly instances of blob, don't need to modifying.

        public BlobService(string ConnectionString)
        {
            blobServiceClient = new(ConnectionString);

        }
        
        // make it a task so it can run in the background without making the rest of the app wait.
        public async Task<string> UploadImageAsync(IFormFile imageToUpload)
        {
            //Opens the folder to store stuff in it
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            // create the folder if it doesn't exist.

            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(Guid.NewGuid().ToString() + Path.GetExtension(imageToUpload.FileName));
            // now we open the image wew want to open, convert into a stream of data then upload it

            using var stream = imageToUpload.OpenReadStream();
            //upload time

            await blobClient.UploadAsync(stream, true);
            //return the url so we cam store it in a database

            return blobClient.Uri.ToString(); 
        }

    }
}
