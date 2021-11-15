using MerchandiseService.Domain.Exceptions;
using MerchandiseService.Domain.Models;

namespace MerchandiseService.Domain.AggregationModels.MerchItemAggregate
{
    public class Size : Enumeration
    {
        public static Size XS = new(1, nameof(XS));
        public static Size S = new(2, nameof(S));
        public static Size M = new(3, nameof(M));
        public static Size L = new(4, nameof(L));
        public static Size XL = new(5, nameof(XL));
        public static Size XXL = new(6, nameof(XXL));

        private Size(int id, string name) : base(id, name)
        {
        }

        public static Size CreateSize(string size)
        {
            switch (size)
            {
                case "XS":
                    return XS;
                case "S":
                    return S;
                case "M":
                    return M;
                case "L":
                    return L;
                case "XL":
                    return XL;
                case "XXL":
                    return XXL;
                case "":
                    return null;
                default:
                    throw new WrongSizeException($"Size {size} does not exist!");
            }
        }
    }
}