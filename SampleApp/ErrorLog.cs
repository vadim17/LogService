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
        private readonly IConfigurationSection _section;

        public ErrorLog(IConfiguration config, RequestDelegate next) : base(next)
        {
            _section = config.GetSection("StorageAccount");
        }        

        protected override CloudStorageAccount GetStorageAccount()
        {            
            return new CloudStorageAccount(new StorageCredentials(_section["Name"], _section["Key"]), true);
        }

        protected override async Task<CloudTable> GetTableAsync(CloudStorageAccount storageAccount)
        {
            var client = storageAccount.CreateCloudTableClient();            
            var table = client.GetTableReference(_section["TableName"]);
            await table.CreateIfNotExistsAsync();
            return table;
        }
    }
}
