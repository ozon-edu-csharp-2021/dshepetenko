using System.Collections.Generic;
using MerchandiseService.Domain.Exceptions;
using MerchandiseService.Domain.Models;

namespace MerchandiseService.Domain.AggregationModels.MerchItemAggregate
{
    public class Quantity : ValueObject
    {
        public Quantity(int value)
        {
            if (value < 0)
            {
                throw new NegativeValueException($"{value} is less than zero. Quantity should be greater than zero.");
            }

            Value = value;
        }

        public int Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}