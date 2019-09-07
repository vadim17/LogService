using LogService.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace SampleApp
{
    public class ErrorLog : LoggingBase
    {
        private readonly IConfiguration _config;

        public ErrorLog(IConfiguration config, RequestDelegate next) : base(next)
        {
            _config = config;
        }        

        protected override CloudStorageAccount GetStorageAccount()
        {
            var section = _config.GetSection("Storage");
            return new CloudStorageAccount(new StorageCredentials(section["Name"], section["Key"]), true);
        }

        protected override async Task<CloudTable> GetTableAsync(CloudStorageAccount storageAccount)
        {
            var client = storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference("");
            await table.CreateIfNotExistsAsync();
            return table;
        }
    }
}
