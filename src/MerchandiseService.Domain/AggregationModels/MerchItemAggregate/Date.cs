using System;
using System.Collections.Generic;
using MerchandiseService.Domain.Models;

namespace MerchandiseService.Domain.AggregationModels.MerchItemAggregate
{
    public class Date : ValueObject
    {
        public Date(DateTime value)
        {
            Value = value;
        }

        public DateTime Value { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public void SetDate(DateTime date)
        {
            Value = date;
        }
    }
}