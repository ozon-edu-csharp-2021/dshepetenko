using MerchandiseService.Domain.AggregationModels.MerchItemAggregate;
using MerchandiseService.Domain.AggregationModels.ValueObjects;
using MerchandiseService.Domain.Models;

namespace MerchandiseService.Domain.AggregationModels.GetAvailableQuantityRequestAggregate
{
    public class GetAvailableQuantityRequest : Entity
    {
        public GetAvailableQuantityRequest(Sku sku)
        {
            Sku = sku;
        }

        public Sku Sku { get; }
    }
}