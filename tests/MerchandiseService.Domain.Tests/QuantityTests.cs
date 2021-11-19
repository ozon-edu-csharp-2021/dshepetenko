using MerchandiseService.Domain.AggregationModels.MerchItemAggregate;
using MerchandiseService.Domain.Exceptions;
using Xunit;

namespace MerchandiseService.Domain.Tests
{
    public class QuantityTests
    {
        [Fact]
        public void CreateQuantity()
        {
            var quantity = 150;

            var result = new Quantity(quantity);

            Assert.Equal(quantity, result.Value);
        }

        [Fact]
        public void CreateQuantityWithNegativeValue()
        {
            var quantity = -300;

            Assert.Throws<NegativeValueException>(() => new Quantity(quantity));
        }
    }
}