using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogService.Client.Models
{
    public class LogEntity : TableEntity
    {
        public LogEntity(HttpContext context, Exception exception, Encoding sessionEncoding = null)
        {
            PartitionKey = context.Request.Host.Host;
            RowKey = Guid.NewGuid().ToString();
            UserName = context.User.Identity.Name;
            ClientIP = context.Connection.RemoteIpAddress.ToString();
            Method = context.Request.Method;
            Path = context.Request.Path;
            Query = context.Request.Query.ToDictionary(item => item.Key, item => string.Join(", ", item.Value));
            Form = context.Request.Form.ToDictionary(item => item.Key, item => string.Join(", ", item.Value));
            Cookies = context.Request.Cookies.ToDictionary(item => item.Key, item => item.Value);
            Session = SessionToDictionary(context.Session, sessionEncoding);
            Message = exception.Message;
            StackTrace = exception.StackTrace;
        }

        private static Dictionary<string, string> SessionToDictionary(ISession session, Encoding encoding = null)
        {
            var result = new Dictionary<string, string>();

            foreach (var key in session.Keys)
            {
                if (session.TryGetValue(key, out byte[] value))
                {
                    result.Add(key, (encoding ?? Encoding.UTF8).GetString(value));
                }
            }

            return result;
        }

        public string UserName { get; set; }

        public string ClientIP { get; set; }

        /// <summary>
        /// Post, Get, etc
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Portion of URL between host and query string
        /// </summary>
        public string Path { get; set; }

        public Dictionary<string, string> Query { get; set; }

        public Dictionary<string, string> Form { get; set; }

        public Dictionary<string, string> Cookies { get; set; }

        public Dictionary<string, string> Session { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }
    }
}
