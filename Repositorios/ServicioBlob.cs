using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conexiones
{
    public class ServicioBlob
    {
        private BlobServiceClient blobServiceClient;
        private string container = "";

        public ServicioBlob()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            this.blobServiceClient = new BlobServiceClient(config.GetConnectionString("ConexionBlob"));
        }

        public async Task UploadBlobAsync(string blobName, Stream content)
        {
            var blobClient = blobServiceClient.GetBlobContainerClient(container).GetBlobClient(blobName);
            await blobClient.UploadAsync(content);
        }

        public async Task DeleteBlobAsync(string blobName)
        {
            var blobClient = blobServiceClient.GetBlobContainerClient(container).GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<Stream> GetBlobAsync(string blobName)
        {
            var blobClient = blobServiceClient.GetBlobContainerClient(container).GetBlobClient(blobName);
            var downloadInfo = await blobClient.DownloadAsync();
            return downloadInfo.Value.Content;
        }

        public async Task<IEnumerable<string>> GetAllBlobsAsync()
        {
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(container);
            var blobs = new List<string>();

            await foreach (var blobItem in blobContainerClient.GetBlobsAsync())
            {
                blobs.Add(blobItem.Name);
            }

            return blobs;
        }

        public async Task<IEnumerable<string>> GetAllBlobsWithConditionAsync(string condition)
        {
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(container);
            var blobs = new List<string>();

            await foreach (var blobItem in blobContainerClient.GetBlobsAsync())
            {
                if (blobItem.Name.Contains(condition))
                {
                    blobs.Add(blobItem.Name);
                }
            }

            return blobs;
        }

    }
}
