using System;
using MerchandiseService.Domain.AggregationModels.MerchItemAggregate;
using MerchandiseService.Domain.AggregationModels.ValueObjects;
using Xunit;

namespace MerchandiseService.Domain.Tests
{
    public class MerchItemTests
    {
        [Fact]
        public void AddDateToMerch()
        {
            var merch = new MerchItem(
                new Name("Super cute sweatshirt"),
                Size.XXL,
                new Sku(112233),
                MerchType.Sweatshirt,
                new Quantity(3), null);

            var date = DateTime.Now;
            
            merch.SetDateOfIssue(date);
            
            Assert.Equal(date, merch.DateOfIssue);
        }
    }
}