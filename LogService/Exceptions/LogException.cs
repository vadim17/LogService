using System;

namespace LogService.Client.Exceptions
{
    public class LogException : Exception
    {
        public LogException(Exception innerException) : base($"Logging exception: {innerException.Message}", innerException)
        {
        }
    }
}
