# Database Seeder

The `DatabaseSeeder` class provides functionality to seed the database with initial data for development and testing purposes.

## Features

- **Role Seeding**: Automatically creates application roles (Admin)
- **Admin User Creation**: Creates a default admin user with configurable credentials
- **Sample Data**: Optionally seeds sample categories and blog posts for testing

## Configuration

The seeder is configured via `appsettings.json` or `appsettings.Development.json`:

```json
{
  "DatabaseSeeder": {
    "Enabled": true,                    // Enable/disable seeding
    "SeedSampleData": true,             // Seed sample categories and posts
    "AdminEmail": "",
    "AdminPassword": "",    // Must meet password requirements
    "AdminFirstName": "Admin",
    "AdminLastName": "User"
  }
}
```

## Usage

The seeder runs automatically at application startup if `DatabaseSeeder:Enabled` is set to `true`. It executes after database migrations are applied.

### Manual Execution

You can also call the seeder programmatically:

```csharp
using var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();

// Seed only roles and admin user
await seeder.SeedAsync(seedSampleData: false);

// Seed roles, admin user, and sample data
await seeder.SeedAsync(seedSampleData: true);
```

## Security Notes

- **Production**: Set `DatabaseSeeder:Enabled` to `false` in production environments
- **Passwords**: The admin password must meet the application's password requirements:
  - Minimum 12 characters
  - At least one digit
  - At least one lowercase letter
  - At least one uppercase letter
  - At least one non-alphanumeric character
- **Credentials**: Store sensitive credentials in User Secrets or environment variables, not in `appsettings.json`

## What Gets Seeded

### Roles
- `Admin` role

### Admin User
- Email: Configurable (default: `admin@shirebudgeters.com`)
- Password: Configurable (default: `Admin@123456`)
- Email confirmed: `true`
- Account active: `true`
- Assigned to `Admin` role

### Sample Data (if enabled)
- **Categories**: Budgeting, Saving Money, Investing
- **Blog Posts**: Sample posts with content and metadata

## Idempotency

The seeder is idempotent - it checks for existing data before seeding:
- Roles are only created if they don't exist
- Admin user is only created if no user with the configured email exists
- Sample categories and posts are only created if they don't already exist

This allows the seeder to be run multiple times safely.

