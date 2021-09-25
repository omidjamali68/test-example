using System;
using System.ComponentModel.DataAnnotations;

namespace Cooking.Services.UserManagement.Contracts
{
    public class SetUserTimeZoneDto
    {
        private Guid userId;

        [Required] public string ZoneName { get; set; }

        [Required] public string LanguageCode { get; set; }

        public Guid GetUserId()
        {
            return userId;
        }

        public void SetUserId(Guid userId)
        {
            this.userId = userId;
        }
    }
}