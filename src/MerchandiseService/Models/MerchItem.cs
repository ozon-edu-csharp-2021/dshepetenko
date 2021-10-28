namespace MerchandiseService.Models
{
    public class MerchItem
    {
        public MerchItem(string name, string size, long sku, int quantity)
        {
            Name = name;
            Size = size;
            Sku = sku;
            Quantity = quantity;
        }
        public string Name { get; }
        public string Size { get; }
        public long Sku { get; }
        public int Quantity { get; }
    }
}