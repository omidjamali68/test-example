using System;

namespace Cooking.Infrastructure
{
    public class VerificationCode
    {
        public static uint Generate()
        {
            var random = new Random();
            var verificationCode = (uint) random.Next(11111, 99999);
            return verificationCode;
        }
    }
}