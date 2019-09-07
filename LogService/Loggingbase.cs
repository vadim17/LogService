using LogService.Client.Exceptions;
using LogService.Client.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Net;
using System.Threading.Tasks;

namespace LogService.Client
{
    public abstract class LoggingBase
    {
        private readonly RequestDelegate _next;

        public LoggingBase(RequestDelegate next)
        {
            _next = next;
        }

        protected abstract CloudStorageAccount GetStorageAccount();
        protected abstract Task<CloudTable> GetTableAsync(CloudStorageAccount storageAccount);

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception exc)
            {
                await LogExceptionAsync(context, exc);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(new 
                {
                    context.Response.StatusCode,
                    Message = exc.Message
                }.ToString());
            }
        }

        private async Task LogExceptionAsync(HttpContext context, Exception exc)
        {
            try
            {
                var entity = new ExceptionEntity(context, exc);
                var account = GetStorageAccount();
                var table = await GetTableAsync(account);
                var operation = TableOperation.Insert(entity);
                await table.ExecuteAsync(operation);
            }
            catch (Exception inner)
            {
                throw new LogException(inner);
            }
        }
    }
}
