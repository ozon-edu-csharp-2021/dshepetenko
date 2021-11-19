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

        public void GiveMerchFromExpected(Sku sku)
        {
            if (ExpectedMerchItems.Count(x => Equals(x.Sku, sku)) == 0)
            {
                throw new NonExistentMerchException($"The employee doesn't expect merch with sku = {sku}");
            }

            var expectedMerch = ExpectedMerchItems.First(x => x.Sku.Equals(sku));
            ExpectedMerchItems.Remove(expectedMerch);
            expectedMerch.SetDateOfIssue(DateTime.Now);
            GivenMerchItems.Add(expectedMerch);
        }

        public bool IsPossibleToIssue(Sku sku)
        {
            if (GivenMerchItems.Count(x => Equals(x.Sku, sku)) == 0)
            {
                return true;
            }

            var dateOfIssue = GivenMerchItems.First(x => Equals(x.Sku, sku)).DateOfIssue;
            if (dateOfIssue != null)
            {
                DateTime dateOfIssueOfThisMerch = (DateTime)dateOfIssue;

                if (DateTime.Now.Subtract(dateOfIssueOfThisMerch).Days > 365)
                {
                    return true;
                }

                return false;
            }

            throw new NoDateException($"Merch Item with Sku = {sku} must have date of issue.");
        }
    }
}