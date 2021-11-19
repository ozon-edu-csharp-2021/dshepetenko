using System;

namespace MerchandiseService.Infrastructure.Repositories.Models
{
    public class MerchItem
    {
        public int Quantity { get; set; }
        
        public DateTime DateOfIssue { get; set; }
    }
}