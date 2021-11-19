namespace MerchandiseService.Models
{
    public class MerchItem
    {
        public string Name { get; init; }
        public string Size { get; init; }
        public long Sku { get; init; }
        public int MerchType { get; init; }
        public int Quantity { get; init; }
    }
}