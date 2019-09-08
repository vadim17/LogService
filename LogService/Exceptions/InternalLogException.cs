using System;

namespace LogService.Client.Exceptions
{
    public class InternalLogException : Exception
    {
        /// <summary>
        /// Thrown when the Azure table storage logging fails
        /// </summary>        
        public InternalLogException(Exception innerException) : base($"Internal logging exception: {innerException.Message}", innerException)
        {
        }
    }
}
