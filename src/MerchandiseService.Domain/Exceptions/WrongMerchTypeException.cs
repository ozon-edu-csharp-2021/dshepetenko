using System;

namespace MerchandiseService.Domain.Exceptions
{
    public class WrongMerchTypeException : Exception
    {
        public WrongMerchTypeException(string message) : base(message)
        {
        }

        public WrongMerchTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}