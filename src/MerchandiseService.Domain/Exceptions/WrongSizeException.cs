using System;

namespace MerchandiseService.Domain.Exceptions
{
    public class WrongSizeException : Exception
    {
        public WrongSizeException(string message) : base(message)
        {
        }

        public WrongSizeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}