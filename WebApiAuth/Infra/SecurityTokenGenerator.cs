using System;

namespace WebApiAuth.Infra
{
    public static class SecurityTokenGenerator
    {
        static SecurityTokenGenerator()
        {
            SecretKey = Guid.NewGuid().ToString();
        }

        public static string SecretKey { get; }
    }
}