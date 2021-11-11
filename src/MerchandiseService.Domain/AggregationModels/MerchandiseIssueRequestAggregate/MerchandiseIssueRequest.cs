using System.Collections.Generic;
using MerchandiseService.Domain.AggregationModels.ValueObjects;
using MerchandiseService.Domain.Models;

namespace MerchandiseService.Domain.AggregationModels.MerchandiseIssueRequestAggregate
{
    public class MerchandiseIssueRequest : Entity
    {
        public MerchandiseIssueRequest(IReadOnlyList<Sku> skuCollection)
        {
            SkuCollection = skuCollection;
        }

        public IReadOnlyList<Sku> SkuCollection { get; }
    }
}