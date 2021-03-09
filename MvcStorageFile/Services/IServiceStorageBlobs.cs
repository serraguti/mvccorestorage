using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStorage.Services
{
    public interface IServiceStorageBlob
    {
        public Task CreateContainerAsync(String containername);
        public Task DeleteContainerAsync(String containername);
        public Task<List<String>> GetContainersAsync();
        public Task UploadBlobAsync(String containerName, String filename
            , String path);

        public Task DeleteBlobAsync(String containerName, String filename);
        public List<String> GetBlobsAsync(String containerName);
    }
}
