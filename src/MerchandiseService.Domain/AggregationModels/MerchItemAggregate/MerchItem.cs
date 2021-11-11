using System;
using MerchandiseService.Domain.AggregationModels.ValueObjects;
using MerchandiseService.Domain.Models;

namespace MerchandiseService.Domain.AggregationModels.MerchItemAggregate
{
    public class MerchItem : Entity
    {
        public MerchItem(Name name, Size size, Sku sku, Quantity quantity, Date dateOfIssue)
        {
            Name = name;
            Size = size;
            Sku = sku;
            Quantity = quantity;
            DateOfIssue = dateOfIssue;
        }

        public Name Name { get; }
        public Size Size { get; }
        public Sku Sku { get; }
        public Quantity Quantity { get; }
        
        public Date DateOfIssue { get; }
    }
}