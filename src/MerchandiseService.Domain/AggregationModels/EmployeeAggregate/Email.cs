using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MerchandiseService.Domain.Models;

namespace MerchandiseService.Domain.AggregationModels.EmployeeAggregate
{
    public class Email : ValueObject
    {
        private Email(string value)
        {
            Value = value;
        }

        public string Value { get; }

        private static bool IsValidEmail(string emailString)
            => Regex.IsMatch(emailString, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

        public static Email Create(string email)
        {
            if (IsValidEmail(email))
            {
                return new Email(email);
            }

            throw new Exception($"{email} is invalid!");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}