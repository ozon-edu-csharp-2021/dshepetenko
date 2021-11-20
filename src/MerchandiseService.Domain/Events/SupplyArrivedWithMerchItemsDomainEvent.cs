using MediatR;
using MerchandiseService.Domain.AggregationModels.MerchItemAggregate;
using MerchandiseService.Domain.AggregationModels.ValueObjects;

namespace MerchandiseService.Domain.Events
{
    public class SupplyArrivedWithMerchItemsDomainEvent : INotification
    {
        public long StockItemSku { get; }
        public int Quantity { get; }

        public SupplyArrivedWithMerchItemsDomainEvent(long stockItemSku,
            int quantity)
        {
            StockItemSku = stockItemSku;
            Quantity = quantity;
        }
    }
}