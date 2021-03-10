using System;
using System.Collections.Generic;
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

        public async Task DeleteBlobAsync(string containerName, string filename)
        {
            throw new NotImplementedException();
        }


        //public async List<String> GetBlobsAsync(string containerName)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task UploadBlobAsync(string containerName, string filename, string path)
        {
            throw new NotImplementedException();
        }
    }
}
