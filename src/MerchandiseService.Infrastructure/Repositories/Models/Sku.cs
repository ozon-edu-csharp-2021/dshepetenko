namespace MerchandiseService.Infrastructure.Repositories.Models
{
    public class Sku
    {
        public string Name { get; set; }
        
        public int ClothingSize { get; set; }
        
        public long Id { get; set; }

        public int MerchTypeId { get; set; }
        
    }
}