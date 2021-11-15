using System;

namespace MerchandiseService.Domain.Exceptions
{
    public class NoDateException : Exception
    {
        public NoDateException(string message) : base(message)
        {
        }
        
        public NoDateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}