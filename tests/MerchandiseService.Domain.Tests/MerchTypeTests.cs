using MerchandiseService.Domain.AggregationModels.MerchItemAggregate;
using MerchandiseService.Domain.Exceptions;
using Xunit;

namespace MerchandiseService.Domain.Tests
{
    public class MerchTypeTests
    {
        [Fact]
        public void GetTypeById()
        {
            var id = 3;
            var result = MerchType.Notepad;
            Assert.Equal(MerchType.GetTypeById(id), result);
        }

        [Fact]
        public void GetTypeByIdWithNegativeValue()
        {
            var id = -1;
            Assert.Throws<WrongMerchTypeException>(() => MerchType.GetTypeById(id));
        }
        
        [Fact]
        public void GetTypeByIdWithWrongValue()
        {
            var id = 7;
            Assert.Throws<WrongMerchTypeException>(() => MerchType.GetTypeById(id));
        }
    }
}