using MerchandiseService.Domain.AggregationModels.MerchItemAggregate;
using MerchandiseService.Domain.Exceptions;
using Xunit;

namespace MerchandiseService.Domain.Tests
{
    public class SizeTests
    {
        [Fact]
        public void CreateSizeByCorrectName()
        {
            var name = "S";
            var result = Size.S;
            Assert.Equal(Size.CreateSize(name), result);
        }

        [Fact]
        public void ReturnNullIfNothingIsGiven()
        {
            var name = "";

            Assert.Null(Size.CreateSize(name));
        }

        [Fact]
        public void CreateSizeByIncorrectName()
        {
            var name = "ExxxtraLarge";

            Assert.Throws<WrongSizeException>(() => Size.CreateSize(name));
        }
    }
}