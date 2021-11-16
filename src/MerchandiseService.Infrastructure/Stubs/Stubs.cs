using System;

namespace MerchandiseService.Infrastructure.Stubs
{
    public class Stubs
    {
        public static int StubGetAvailableQuantity(long sku)
        {
            return new Random().Next(150);
        }
    }
}