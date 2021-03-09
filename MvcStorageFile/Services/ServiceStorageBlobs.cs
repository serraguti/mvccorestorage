using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace MvcStorage.Services
{
    public class ServiceStorageBlobs : IServiceStorageBlob
    {
        private BlobServiceClient service;
        public ServiceStorageBlobs(String keys)
        {
             this.service =
                new BlobServiceClient(keys);
        }
        public async Task<List<string>> GetContainersAsync()
        {
            BlobContainerClient client = service.GetBlobContainerClient("fotos1");
            await client.GetBlobsAsync(Azure.Storage.Blobs.Models.BlobTraits.All
                , Azure.Storage.Blobs.Models.BlobStates.None);
        }

        public async Task CreateContainerAsync(string containername)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteBlobAsync(string containerName, string filename)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteContainerAsync(string containername)
        {
            throw new NotImplementedException();
        }

        public async List<String> GetBlobsAsync(string containerName)
        {
            throw new NotImplementedException();
        }

        public async Task UploadBlobAsync(string containerName, string filename, string path)
        {
            throw new NotImplementedException();
        }
    }
}
