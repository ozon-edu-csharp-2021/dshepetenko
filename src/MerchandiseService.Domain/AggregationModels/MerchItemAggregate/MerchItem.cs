using System;
using MerchandiseService.Domain.AggregationModels.ValueObjects;
using MerchandiseService.Domain.Models;

namespace MerchandiseService.Domain.AggregationModels.MerchItemAggregate
{
    public class MerchItem : Entity
    {
        public MerchItem(Name name, Size size, Sku sku, MerchType merchType, Quantity quantity, DateTime? dateOfIssue)
        {
            Name = name;
            Size = size;
            Sku = sku;
            MerchType = merchType;
            Quantity = quantity;
            DateOfIssue = dateOfIssue;
        }

        public Name Name { get; }
        public Size Size { get; }
        public Sku Sku { get; }
        public MerchType MerchType { get; }
        public Quantity Quantity { get; }
        public DateTime? DateOfIssue { get; private set; }

        public void SetDateOfIssue(DateTime date)
        {
            DateOfIssue = date;
        }
    }
}