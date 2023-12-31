﻿using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using IRepositorios;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;

namespace Conexiones
{
    public class ServicioBlobAzure
    {
        private BlobServiceClient blobServiceClient;
        private string _container = "imagenes";
        private string _cuenta = "repoimagenes";
        private string _llave = "6qaA4JgbFAW01KExpXDXhwB/+s8LSfu05gqoo4KP3CnBgYAqW62NSPlZY5/K96mitDLu9vcjKuD8+AStbIjPZA==";

        public ServicioBlobAzure()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var credential = new StorageSharedKeyCredential(_cuenta, _llave);
            var blobUri = $"https://{_cuenta}.blob.core.windows.net";
            this.blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
        }

        public async Task UploadBlobAsync(string blobName, Stream content)
        {
            var blobClient = blobServiceClient.GetBlobContainerClient(_container).GetBlobClient(blobName);
            await blobClient.UploadAsync(content);
        }

        public async Task DeleteBlobAsync(string blobName)
        {
            var blobClient = blobServiceClient.GetBlobContainerClient(_container).GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<byte[]> GetBlobAsync(string blobName)
        {
            var blobClient = blobServiceClient.GetBlobContainerClient(_container).GetBlobClient(blobName);
            if (await blobClient.ExistsAsync())
            {
                BlobDownloadInfo download = await blobClient.DownloadAsync();
                using var memoryStream = new MemoryStream();
                await download.Content.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }

            return null;
        }

        public async Task<IEnumerable<string>> GetAllBlobsAsync()
        {
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_container);
            var blobs = new List<string>();

            await foreach (var blobItem in blobContainerClient.GetBlobsAsync())
            {
                blobs.Add(blobItem.Name);
            }

            return blobs;
        }

        public async Task<IEnumerable<string>> GetAllBlobsWithConditionAsync(string condition)
        {
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_container);
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
