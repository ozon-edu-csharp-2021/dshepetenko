using MerchandiseService.Domain.Exceptions;
using MerchandiseService.Domain.Models;

namespace MerchandiseService.Domain.AggregationModels.MerchItemAggregate
{
    public class MerchType : Enumeration
    {
        public static MerchType TShirt = new(1, nameof(TShirt));
        public static MerchType Sweatshirt = new(2, nameof(Sweatshirt));
        public static MerchType Notepad = new(3, nameof(Notepad));
        public static MerchType Bag = new(4, nameof(Bag));
        public static MerchType Pen = new(5, nameof(Pen));
        public static MerchType Socks = new(6, nameof(Socks));

        private MerchType(int id, string name) : base(id, name)
        {
        }

        public static MerchType GetTypeById(int id)
        {
            switch (id)
            {
                case 1 :
                    return  TShirt;
                case 2:
                    return Sweatshirt;
                case 3:
                    return Notepad;
                case 4:
                    return Bag;
                case 5:
                    return Pen;
                case 6:
                    return Socks;
                default:
                    throw new WrongMerchTypeException($"Merch type with if = {id} does not exist!");
            }
        }
    }
}