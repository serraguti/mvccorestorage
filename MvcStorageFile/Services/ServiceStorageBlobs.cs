using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MvcStorage.Services
{
    public class ServiceStorageBlobs 
    {
        private BlobServiceClient service;

        public ServiceStorageBlobs(String keys)
        {
             this.service =
                new BlobServiceClient(keys);
        }

        public async Task<List<string>> GetContainersAsync()
        {
            List<String> containers = new List<string>();
            await foreach (BlobContainerItem container in
                this.service.GetBlobContainersAsync())
            {
                containers.Add(container.Name);
            }
            return containers;
        }

        public async Task CreateContainerAsync(string containername)
        {
            await this.service.CreateBlobContainerAsync(containername);
        }

        public async Task DeleteContainerAsync(string containername)
        {
            await this.service.DeleteBlobContainerAsync(containername);
        }

        public async Task<List<String>> GetBlobsAsync(string containername)
        {
            BlobContainerClient containerClient =
                this.service.GetBlobContainerClient(containername);
            List<String> blobs = new List<string>();
            await foreach (BlobItem blob in containerClient.GetBlobsAsync())
            {
                blobs.Add(blob.Name);
            }
            return blobs;
        }

        public async Task DeleteBlobAsync(string containername
            , string blobname)
        {
            BlobContainerClient containerClient =
                this.service.GetBlobContainerClient(containername);
            await containerClient.DeleteBlobAsync(blobname);
        }

        public async Task UploadBlobAsync(string containername
            , string blobname, Stream stream)
        {
            BlobContainerClient containerClient =
                this.service.GetBlobContainerClient(containername);
            await
                containerClient.UploadBlobAsync(blobname, stream);
        }
    }
}
