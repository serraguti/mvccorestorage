using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Files.Shares;

namespace MvcStorageFile.Services
{
    public class ServiceStorageFile
    {
        private ShareClient client;

        public ServiceStorageFile()
        {
            String keys = "DefaultEndpointsProtocol=https;AccountName=storagetajamarpgs;AccountKey=AZN0grugwrgClnhivSv5up4VD2OAwT2m8Hk4j1z3BZ+MAYUAIUBc3T6JaQHL1vDSP6zKJikQm3bFB+gob1gMMQ==;EndpointSuffix=core.windows.net";
            this.client =
                new ShareClient(keys, "ejemplo");
        }
    }
}
