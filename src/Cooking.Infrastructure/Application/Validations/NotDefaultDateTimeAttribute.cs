using System;
using System.ComponentModel.DataAnnotations;

namespace Cooking.Infrastructure.Application.Validations
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class NotDefaultDateTimeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime dateTime) return dateTime != default;

            return true;
        }
    }
}