using MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using MerchandiseService.Domain.Exceptions;
using Xunit;

namespace MerchandiseService.Domain.Tests
{
    public class EmailTests
    {
        [Fact]
        public void CreateCorrectEmail()
        {
            var email = "example@example.com";
            Assert.Equal(email, Email.Create(email).Value);
        }

        [Fact]
        public void CreateWrongEmail()
        {
            var email = "some @@incorrectemail.ru,";
            Assert.Throws<InvalidEmailException>(() => Email.Create(email));
        }
    }
}