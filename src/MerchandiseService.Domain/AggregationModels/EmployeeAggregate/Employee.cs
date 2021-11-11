using System;
using System.Collections.Generic;
using System.Linq;
using MerchandiseService.Domain.AggregationModels.MerchItemAggregate;
using MerchandiseService.Domain.AggregationModels.ValueObjects;
using MerchandiseService.Domain.Exceptions;
using MerchandiseService.Domain.Models;

namespace MerchandiseService.Domain.AggregationModels.EmployeeAggregate
{
    public class Employee : Entity
    {
        public Employee(EmployeeId employeeId, Email email, List<MerchItem> givenMerchItems,
            List<MerchItem> expectedMerchItems)
        {
            EmployeeId = employeeId;
            Email = email;
            GivenMerchItems = givenMerchItems;
            ExpectedMerchItems = expectedMerchItems;
        }

        public EmployeeId EmployeeId { get; }

        public Email Email { get; }

        public List<MerchItem> GivenMerchItems { get; }

        public List<MerchItem> ExpectedMerchItems { get; }

        private void GiveMerchFromExpected(Sku sku)
        {
            if (ExpectedMerchItems.Count(x => Equals(x.Sku, sku)) == 0)
            {
                throw new NonExistentMerchException($"The employee doesn't expect merch with sku = {sku}");
            }

            var expectedMerch = ExpectedMerchItems.First(x => x.Sku.Equals(sku));
            ExpectedMerchItems.Remove(expectedMerch);
            expectedMerch.DateOfIssue.SetDate(DateTime.Now);
            GivenMerchItems.Add(expectedMerch);
        }
    }
}