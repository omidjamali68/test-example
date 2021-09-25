using Cooking.Infrastructure.Entities;

namespace Cooking.Services.StateServices.Provinces.Exceptions
{
    public class DuplicateProvinceNameException : BusinessException
    {
        public string Name { get; set; }
    }
}