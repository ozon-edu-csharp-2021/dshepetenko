using System;
using System.Collections.Generic;
using System.Linq;
using MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using MerchandiseService.Domain.AggregationModels.MerchItemAggregate;
using MerchandiseService.Domain.AggregationModels.ValueObjects;
using MerchandiseService.Domain.Exceptions;
using Xunit;

namespace MerchandiseService.Domain.Tests
{
    public class EmployeeTests
    {
        [Fact]
        public void GiveMerchFromExpected()
        {
            var given = new List<MerchItem>()
            {
                new MerchItem(new Name("n"), Size.L, new Sku(13), MerchType.Bag, new Quantity(15),
                    new DateTime(2020, 12, 12)),
                new MerchItem(new Name("b"), Size.L, new Sku(18), MerchType.Notepad, new Quantity(15),
                    new DateTime(2018, 12, 19)),
                new MerchItem(new Name("v"), Size.L, new Sku(12), MerchType.Socks, new Quantity(15),
                    new DateTime(2020, 5, 12)),
            };
            var expected = new List<MerchItem>()
            {
                new MerchItem(new Name("c"), Size.L, new Sku(19), MerchType.Sweatshirt, new Quantity(15), null),
                new MerchItem(new Name("x"), Size.L, new Sku(1), MerchType.TShirt, new Quantity(15), null),
            };
            var employee = new Employee(
                new EmployeeId(12324),
                Email.Create("example@example.com"),
                given,
                expected);
            var sku = new Sku(1);
            employee.GiveMerchFromExpected(sku);
            Assert.True(employee.ExpectedMerchItems.Count(x => x.Sku.Equals(sku)) == 0);
            Assert.True(employee.GivenMerchItems.Count(x => x.Sku.Equals(sku)) == 1);
            Assert.True(employee.GivenMerchItems.First(x => x.Sku.Equals(sku)).DateOfIssue != null);
        }

        [Fact]
        public void IsPossibleToIssue()
        {
            var given = new List<MerchItem>()
            {
                new MerchItem(new Name("n"), Size.L, new Sku(13), MerchType.Bag, new Quantity(15),
                    new DateTime(2020, 12, 12)),
                new MerchItem(new Name("b"), Size.L, new Sku(18), MerchType.Notepad, new Quantity(15),
                    new DateTime(2018, 12, 19)),
                new MerchItem(new Name("v"), Size.L, new Sku(12), MerchType.Socks, new Quantity(15),
                    new DateTime(2020, 5, 12)),
            };
            var expected = new List<MerchItem>()
            {
                new MerchItem(new Name("c"), Size.L, new Sku(19), MerchType.Sweatshirt, new Quantity(15), null),
                new MerchItem(new Name("x"), Size.L, new Sku(1), MerchType.TShirt, new Quantity(15), null),
            };
            var employee = new Employee(
                new EmployeeId(12324),
                Email.Create("example@example.com"),
                given,
                expected);
            var first = new Sku(18);
            var second = new Sku(242352);
            Assert.True(employee.IsPossibleToIssue(first));
            Assert.True(employee.IsPossibleToIssue(second));
        }

        [Fact]
        public void IsImpossibleToIssue()
        {
            var given = new List<MerchItem>()
            {
                new MerchItem(new Name("n"), Size.L, new Sku(13), MerchType.Bag, new Quantity(15),
                    new DateTime(2020, 12, 12)),
                new MerchItem(new Name("b"), Size.L, new Sku(18), MerchType.Notepad, new Quantity(15),
                    new DateTime(2018, 12, 19)),
                new MerchItem(new Name("v"), Size.L, new Sku(12), MerchType.Socks, new Quantity(15),
                    new DateTime(2021, 5, 12)),
            };
            var expected = new List<MerchItem>()
            {
                new MerchItem(new Name("c"), Size.L, new Sku(19), MerchType.Sweatshirt, new Quantity(15), null),
                new MerchItem(new Name("x"), Size.L, new Sku(1), MerchType.TShirt, new Quantity(15), null),
            };
            var employee = new Employee(
                new EmployeeId(12324),
                Email.Create("example@example.com"),
                given,
                expected);
            var first = new Sku(12);
            Assert.False(employee.IsPossibleToIssue(first));
        }
        
        [Fact]
        public void WithoutDateOfIssue()
        {
            var given = new List<MerchItem>()
            {
                new MerchItem(new Name("n"), Size.L, new Sku(13), MerchType.Bag, new Quantity(15),
                    null),
                new MerchItem(new Name("b"), Size.L, new Sku(18), MerchType.Notepad, new Quantity(15),
                    new DateTime(2018, 12, 19)),
                new MerchItem(new Name("v"), Size.L, new Sku(12), MerchType.Socks, new Quantity(15),
                    new DateTime(2021, 5, 12)),
            };
            var expected = new List<MerchItem>()
            {
                new MerchItem(new Name("c"), Size.L, new Sku(19), MerchType.Sweatshirt, new Quantity(15), null),
                new MerchItem(new Name("x"), Size.L, new Sku(1), MerchType.TShirt, new Quantity(15), null),
            };
            var employee = new Employee(
                new EmployeeId(12324),
                Email.Create("example@example.com"),
                given,
                expected);
            var first = new Sku(13);
            Assert.Throws<NoDateException>(()=>employee.IsPossibleToIssue(first));
        }
    }
}