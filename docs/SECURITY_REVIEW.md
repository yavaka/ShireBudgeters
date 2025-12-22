# Security Review and Hardening

**Date**: 22/12/2025
**Status**: Completed  
**Priority**: High

## Overview

This document outlines the comprehensive security review and hardening measures implemented for the ShireBudgeters application to meet production security standards.

## Security Measures Implemented

### 1. Password Requirements ✅

**Location**: `ShireBudgeters.BL/Configurations/BusinessLogicConfigurations.cs`

**Configuration**:
- **Minimum Length**: 12 characters (industry standard)
- **Require Digit**: Yes
- **Require Lowercase**: Yes
- **Require Uppercase**: Yes
- **Require Non-Alphanumeric**: Yes (special characters)
- **Required Unique Characters**: 1

**Rationale**: These requirements align with NIST SP 800-63B guidelines and provide strong protection against password-based attacks.

### 2. Lockout Settings ✅

**Location**: `ShireBudgeters.BL/Configurations/BusinessLogicConfigurations.cs`

**Configuration**:
- **Max Failed Access Attempts**: 5
- **Lockout Duration**: 15 minutes
- **Lockout Enabled for New Users**: Yes

**Additional Fix**: Updated `IdentityService.cs` to enable `lockoutOnFailure: true` in the `PasswordSignInAsync` method to ensure automatic lockout after failed attempts.

**Rationale**: Prevents brute force attacks by locking accounts after 5 failed login attempts for 15 minutes.

### 3. Cookie Security Settings ✅

**Location**: `ShireBudgeters.BL/Configurations/BusinessLogicConfigurations.cs`

**Configuration for All Cookies**:
- **HttpOnly**: `true` - Prevents JavaScript access (XSS protection)
- **SameSite**: `Strict` - CSRF protection
- **Secure Policy**: 
  - Development: `SameAsRequest` (allows HTTP for local development)
  - Production: `Always` (HTTPS only)

**Cookie Types Configured**:
- Application Cookie (authentication)
- External Cookie
- Two-Factor Remember Me Cookie
- Two-Factor User ID Cookie

**Session Settings**:
- **Expire TimeSpan**: 8 hours
- **Sliding Expiration**: Enabled (extends session on activity)

**Rationale**: Secure cookies prevent XSS and CSRF attacks while maintaining usability.

### 4. HTTPS Enforcement ✅

**Location**: `ShireBudgeters/Program.cs`

**Configuration**:
- **HSTS (HTTP Strict Transport Security)**: Enabled in production
  - Forces browsers to use HTTPS for 1 year
- **HTTPS Redirection**: Enabled for all environments
  - Automatically redirects HTTP requests to HTTPS

**Rationale**: Ensures all data in transit is encrypted, protecting against man-in-the-middle attacks.

### 5. Security Headers ✅

**Location**: `ShireBudgeters.Common/Middleware/SecurityHeadersMiddleware.cs`

**Headers Implemented**:
- **X-Content-Type-Options**: `nosniff` - Prevents MIME type sniffing
- **X-Frame-Options**: `DENY` - Prevents clickjacking attacks
- **X-XSS-Protection**: `1; mode=block` - Enables XSS filter
- **Referrer-Policy**: `strict-origin-when-cross-origin` - Controls referrer information
- **Permissions-Policy**: Restricts access to browser features (geolocation, microphone, camera)
- **Content-Security-Policy**: Restrictive policy to prevent XSS and data injection attacks

**Rationale**: Security headers provide defense-in-depth against various web vulnerabilities.

### 6. Connection String Security ✅

**Location**: `ShireBudgeters/appsettings.json`

**Current Status**: Connection strings are stored in `appsettings.json` (empty in source control).

**Best Practices Documented**:

#### Development
- Use **User Secrets** for local development:
  ```bash
  dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-connection-string"
  ```

#### Production
- **DO NOT** store connection strings in `appsettings.json` or any source control
- Use one of the following secure methods:
  1. **Azure Key Vault** (recommended for Azure deployments)
  2. **Environment Variables** (set at the hosting level)
  3. **Secret Management Services** (AWS Secrets Manager, HashiCorp Vault, etc.)
  4. **Configuration Providers** that support secure storage

**Example for Azure App Service**:
```bash
az webapp config connection-string set \
  --resource-group <resource-group> \
  --name <app-name> \
  --connection-string-type SQLAzure \
  --settings DefaultConnection="<connection-string>"
```

**Example for Environment Variables**:
```bash
# Linux/Mac
export ConnectionStrings__DefaultConnection="your-connection-string"

# Windows PowerShell
$env:ConnectionStrings__DefaultConnection="your-connection-string"
```

### 7. Additional Security Measures

#### IP Address Logging
- **Location**: `ShireBudgeters.Common/Middleware/IpAddressLoggingMiddleware.cs`
- All requests are logged with IP addresses for security auditing
- Supports proxy headers (X-Forwarded-For, X-Real-IP, CF-Connecting-IP)

#### Antiforgery Protection
- **Location**: `ShireBudgeters/Program.cs`
- Antiforgery tokens are enabled to prevent CSRF attacks

#### Error Handling
- **Location**: `ShireBudgeters/Program.cs`
- Production error pages don't expose sensitive information
- Detailed errors only shown in development

## Security Checklist

- [x] Password requirements meet security standards (12+ chars, complexity)
- [x] Lockout settings prevent brute force attacks (5 attempts, 15 min lockout)
- [x] Cookies are secure in production (HttpOnly, Secure, SameSite=Strict)
- [x] HTTPS enforced (HSTS + HTTPS redirection)
- [x] Connection strings secured (documented best practices)
- [x] Security headers added (X-Content-Type-Options, X-Frame-Options, CSP, etc.)
- [x] Security review documented

## Recommendations for Production Deployment

1. **Connection Strings**: 
   - Migrate to Azure Key Vault or equivalent secret management service
   - Never commit connection strings to source control

2. **Content Security Policy**:
   - Review and adjust CSP based on actual application requirements
   - Test thoroughly to ensure legitimate functionality isn't blocked

3. **Monitoring**:
   - Set up alerts for multiple failed login attempts
   - Monitor for account lockout events
   - Track security-related log events

4. **Regular Security Audits**:
   - Review security settings quarterly
   - Keep dependencies updated
   - Monitor security advisories for ASP.NET Core and related packages

5. **Additional Considerations**:
   - Consider implementing two-factor authentication (2FA)
   - Review and implement rate limiting for API endpoints
   - Consider implementing CAPTCHA for login forms after multiple failures

## Testing

### Password Requirements
- Test password creation with various combinations
- Verify password validation messages are clear
- Test password reset functionality

### Lockout Functionality
- Test account lockout after 5 failed attempts
- Verify lockout duration (15 minutes)
- Test that lockout resets after timeout

### Cookie Security
- Verify cookies are HttpOnly (not accessible via JavaScript)
- Verify cookies use Secure flag in production
- Test SameSite=Strict behavior

### HTTPS Enforcement
- Verify HTTP redirects to HTTPS in production
- Test HSTS header is present in production responses
- Verify HTTPS works correctly

### Security Headers
- Use browser developer tools to verify all headers are present
- Test CSP doesn't break legitimate functionality
- Verify X-Frame-Options prevents iframe embedding

## References

- [NIST SP 800-63B - Digital Identity Guidelines](https://pages.nist.gov/800-63-3/)
- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [ASP.NET Core Security Documentation](https://learn.microsoft.com/en-us/aspnet/core/security/)
- [Content Security Policy Reference](https://developer.mozilla.org/en-US/docs/Web/HTTP/CSP)


