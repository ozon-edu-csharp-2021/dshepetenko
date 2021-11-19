using System;

namespace MerchandiseService.Domain.Exceptions
{
    public class NonExistentMerchException : Exception
    {
        public NonExistentMerchException(string message) : base(message)
        {
        }

        public NonExistentMerchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}