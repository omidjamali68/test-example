﻿using Cooking.Infrastructure.Application.Validations;

namespace Cooking.Services.UserManagement.Contracts
{
    public class ChangeMobileNumberDto
    {
        [Required] public string NationalCode { get; set; }

        [Required] public string CountryCallingCode { get; set; }

        [MobileNumber] [Required] public string MobileNumber { get; set; }
    }
}