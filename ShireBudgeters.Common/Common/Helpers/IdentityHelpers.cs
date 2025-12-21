using System;
using System.Collections.Generic;
using System.Text;

namespace ShireBudgeters.Common.Common.Helpers
{
    public static class IdentityHelpers
    {
        /// <summary>
        /// Masks an email address for logging purposes to protect user privacy.
        /// Example: "user@example.com" -> "u***@e***.com"
        /// </summary>
        public static string MaskEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return "[UNKNOWN]";
            }

            var parts = email.Split('@');
            if (parts.Length != 2)
            {
                return "[INVALID]";
            }

            var localPart = parts[0];
            var domainPart = parts[1];

            // Mask local part: show first character, mask the rest
            var maskedLocal = localPart.Length > 1
                ? $"{localPart[0]}{new string('*', Math.Min(localPart.Length - 1, 3))}"
                : localPart;

            // Mask domain: show first character, mask the rest
            var domainParts = domainPart.Split('.');
            if (domainParts.Length >= 2)
            {
                var domainName = string.Join(".", domainParts.Take(domainParts.Length - 1));
                var tld = domainParts.Last();

                var maskedDomainName = domainName.Length > 1
                    ? $"{domainName[0]}{new string('*', Math.Min(domainName.Length - 1, 3))}"
                    : domainName;

                var maskedDomain = $"{maskedDomainName}.{tld}";
                return $"{maskedLocal}@{maskedDomain}";
            }

            // Fallback for unusual domain formats
            var maskedDomainFallback = domainPart.Length > 1
                ? $"{domainPart[0]}{new string('*', Math.Min(domainPart.Length - 1, 3))}"
                : domainPart;

            return $"{maskedLocal}@{maskedDomainFallback}";
        }

    }
}
