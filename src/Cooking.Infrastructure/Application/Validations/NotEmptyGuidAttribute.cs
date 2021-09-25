using System;
using System.ComponentModel.DataAnnotations;

namespace Cooking.Infrastructure.Application.Validations
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class NotEmptyGuidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is Guid guid) return guid != Guid.Empty;

            return true;
        }
    }
}