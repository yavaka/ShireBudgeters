# Database Schema Documentation

## Overview

This document provides a comprehensive overview of the ShireBudgeters database schema. The database uses SQL Server and follows ASP.NET Core Identity patterns for authentication and authorization. All application tables include audit tracking fields for created/modified information.

**Database System:** SQL Server  
**Identity Framework:** ASP.NET Core Identity  
**Audit Pattern:** All application tables implement audit tracking (CreatedBy, CreatedDate, ModifiedBy, ModifiedDate)

---

## Table of Contents

1. [Identity Tables](#identity-tables)
2. [Application Tables](#application-tables)
3. [Relationships](#relationships)
4. [Indexes](#indexes)
5. [Constraints](#constraints)
6. [Default Values](#default-values)

---

## Identity Tables

### AspNetUsers

The main user table that extends ASP.NET Core Identity with custom fields.

| Column Name | Data Type | Max Length | Nullable | Default | Description |
|------------|-----------|------------|----------|---------|-------------|
| Id | nvarchar | 450 | No | - | Primary Key (GUID string) |
| FirstName | nvarchar | 100 | No | - | User's first name |
| LastName | nvarchar | 100 | No | - | User's last name |
| IsActive | bit | - | No | false | Whether the user account is active |
| LastLoginDate | datetime2 | - | Yes | - | Last successful login timestamp |
| UserName | nvarchar | 256 | Yes | - | Username for login |
| NormalizedUserName | nvarchar | 256 | Yes | - | Normalized username (for case-insensitive lookups) |
| Email | nvarchar | 256 | Yes | - | User's email address |
| NormalizedEmail | nvarchar | 256 | Yes | - | Normalized email (for case-insensitive lookups) |
| EmailConfirmed | bit | - | No | false | Whether email has been confirmed |
| PasswordHash | nvarchar | MAX | Yes | - | Hashed password |
| SecurityStamp | nvarchar | MAX | Yes | - | Security stamp for token validation |
| ConcurrencyStamp | nvarchar | MAX | Yes | - | Concurrency token for optimistic locking |
| PhoneNumber | nvarchar | 256 | Yes | - | User's phone number |
| PhoneNumberConfirmed | bit | - | No | false | Whether phone number has been confirmed |
| TwoFactorEnabled | bit | - | No | false | Whether two-factor authentication is enabled |
| LockoutEnd | datetimeoffset | - | Yes | - | End time for account lockout |
| LockoutEnabled | bit | - | No | false | Whether account lockout is enabled |
| AccessFailedCount | int | - | No | 0 | Number of failed login attempts |
| CreatedBy | nvarchar | 100 | No | - | User who created the record |
| CreatedDate | datetime2 | - | No | GETDATE() | Record creation timestamp |
| ModifiedBy | nvarchar | 100 | No | - | User who last modified the record |
| ModifiedDate | datetime2 | - | No | GETDATE() | Last modification timestamp |

**Primary Key:** Id  
**Unique Indexes:**
- NormalizedUserName (unique, filtered where not null)
- NormalizedEmail (non-unique index)

---

### AspNetRoles

Stores application roles for authorization.

| Column Name | Data Type | Max Length | Nullable | Default | Description |
|------------|-----------|------------|----------|---------|-------------|
| Id | nvarchar | 450 | No | - | Primary Key (GUID string) |
| Name | nvarchar | 256 | Yes | - | Role name |
| NormalizedName | nvarchar | 256 | Yes | - | Normalized role name (for case-insensitive lookups) |
| ConcurrencyStamp | nvarchar | MAX | Yes | - | Concurrency token for optimistic locking |

**Primary Key:** Id  
**Unique Index:** NormalizedName (unique, filtered where not null)

---

### AspNetUserRoles

Junction table linking users to roles (many-to-many relationship).

| Column Name | Data Type | Max Length | Nullable | Default | Description |
|------------|-----------|------------|----------|---------|-------------|
| UserId | nvarchar | 450 | No | - | Foreign Key to AspNetUsers.Id |
| RoleId | nvarchar | 450 | No | - | Foreign Key to AspNetRoles.Id |

**Primary Key:** (UserId, RoleId)  
**Foreign Keys:**
- UserId → AspNetUsers.Id (CASCADE DELETE)
- RoleId → AspNetRoles.Id (CASCADE DELETE)

---

### AspNetUserClaims

Stores custom claims associated with users.

| Column Name | Data Type | Max Length | Nullable | Default | Description |
|------------|-----------|------------|----------|---------|-------------|
| Id | int | - | No | - | Primary Key (Identity) |
| UserId | nvarchar | 450 | No | - | Foreign Key to AspNetUsers.Id |
| ClaimType | nvarchar | MAX | Yes | - | Type of claim |
| ClaimValue | nvarchar | MAX | Yes | - | Value of claim |

**Primary Key:** Id  
**Foreign Key:** UserId → AspNetUsers.Id (CASCADE DELETE)

---

### AspNetRoleClaims

Stores custom claims associated with roles.

| Column Name | Data Type | Max Length | Nullable | Default | Description |
|------------|-----------|------------|----------|---------|-------------|
| Id | int | - | No | - | Primary Key (Identity) |
| RoleId | nvarchar | 450 | No | - | Foreign Key to AspNetRoles.Id |
| ClaimType | nvarchar | MAX | Yes | - | Type of claim |
| ClaimValue | nvarchar | MAX | Yes | - | Value of claim |

**Primary Key:** Id  
**Foreign Key:** RoleId → AspNetRoles.Id (CASCADE DELETE)

---

### AspNetUserLogins

Stores external login providers (e.g., Google, Facebook) for users.

| Column Name | Data Type | Max Length | Nullable | Default | Description |
|------------|-----------|------------|----------|---------|-------------|
| LoginProvider | nvarchar | 128 | No | - | External login provider name |
| ProviderKey | nvarchar | 128 | No | - | Provider-specific key |
| ProviderDisplayName | nvarchar | MAX | Yes | - | Display name for the provider |
| UserId | nvarchar | 450 | No | - | Foreign Key to AspNetUsers.Id |

**Primary Key:** (LoginProvider, ProviderKey)  
**Foreign Key:** UserId → AspNetUsers.Id (CASCADE DELETE)

---

### AspNetUserTokens

Stores authentication tokens for users (e.g., refresh tokens).

| Column Name | Data Type | Max Length | Nullable | Default | Description |
|------------|-----------|------------|----------|---------|-------------|
| UserId | nvarchar | 450 | No | - | Foreign Key to AspNetUsers.Id |
| LoginProvider | nvarchar | 128 | No | - | Login provider name |
| Name | nvarchar | 128 | No | - | Token name |
| Value | nvarchar | MAX | Yes | - | Token value |

**Primary Key:** (UserId, LoginProvider, Name)  
**Foreign Key:** UserId → AspNetUsers.Id (CASCADE DELETE)

---

### AspNetUserPasskeys

Stores WebAuthn/Passkey credentials for passwordless authentication.

| Column Name | Data Type | Max Length | Nullable | Default | Description |
|------------|-----------|------------|----------|---------|-------------|
| CredentialId | varbinary | 1024 | No | - | Primary Key - Unique credential identifier |
| UserId | nvarchar | 450 | No | - | Foreign Key to AspNetUsers.Id |
| Data | nvarchar | MAX | No | - | JSON-encoded passkey data (AttestationObject, PublicKey, etc.) |

**Primary Key:** CredentialId  
**Foreign Key:** UserId → AspNetUsers.Id (CASCADE DELETE)

**Note:** The Data column stores JSON containing:
- AttestationObject
- ClientDataJson
- PublicKey
- CreatedAt
- IsBackedUp
- IsBackupEligible
- IsUserVerified
- Name
- SignCount
- Transports

---

## Application Tables

### Categories

Stores hierarchical categories that can be organized in a tree structure. Each category belongs to a user.

| Column Name | Data Type | Max Length | Nullable | Default | Description |
|------------|-----------|------------|----------|---------|-------------|
| Id | int | - | No | - | Primary Key (Identity, starts at 1) |
| Name | nvarchar | 100 | No | - | Category name |
| Description | nvarchar | 500 | Yes | - | Category description |
| Color | nvarchar | 50 | Yes | - | Color or icon identifier for UI display |
| UserId | nvarchar | 450 | No | - | Foreign Key to AspNetUsers.Id - Owner of the category |
| ParentCategoryId | int | - | Yes | - | Foreign Key to Categories.Id - Parent category for hierarchy |
| IsActive | bit | - | No | true | Whether the category is active |
| CreatedBy | nvarchar | 100 | Yes | - | User who created the record |
| CreatedDate | datetime2 | - | No | GETDATE() | Record creation timestamp |
| ModifiedBy | nvarchar | 100 | Yes | - | User who last modified the record |
| ModifiedDate | datetime2 | - | Yes | GETDATE() | Last modification timestamp |

**Primary Key:** Id  
**Foreign Keys:**
- UserId → AspNetUsers.Id (CASCADE DELETE)
- ParentCategoryId → Categories.Id (RESTRICT DELETE - prevents deletion if child categories exist)

**Unique Constraints:**
- (UserId, Name) - Ensures unique category names per user

**Indexes:**
- IX_Categories_UserId
- IX_Categories_ParentCategoryId
- IX_Categories_UserId_Name (unique)

**Business Rules:**
- Categories can have a parent category, enabling hierarchical organization
- A category cannot be deleted if it has child categories (RESTRICT)
- Category names must be unique per user

---

### BlogPosts

Stores blog posts with content, metadata, and publication information.

| Column Name | Data Type | Max Length | Nullable | Default | Description |
|------------|-----------|------------|----------|---------|-------------|
| Id | int | - | No | - | Primary Key (Identity, starts at 1) |
| AuthorId | nvarchar | 450 | No | - | Foreign Key to AspNetUsers.Id - Post author |
| Title | nvarchar | 255 | No | - | Post title |
| Slug | nvarchar | 255 | No | - | URL-friendly version of title (unique) |
| ContentBody | nvarchar | MAX | Yes | - | Full HTML/Markdown content of the post |
| FeaturedImageUrl | nvarchar | 500 | Yes | - | URL to featured image |
| PublicationDate | datetime2 | - | No | - | Date/time when post was or will be published |
| IsPublished | bit | - | No | false | Whether the post is published (true) or draft (false) |
| CategoryId | int | - | Yes | - | Foreign Key to Categories.Id - Associated category |
| MetaDescription | nvarchar | 300 | Yes | - | SEO description for search engines and social media |
| CreatedBy | nvarchar | 100 | Yes | - | User who created the record |
| CreatedDate | datetime2 | - | No | GETDATE() | Record creation timestamp |
| ModifiedBy | nvarchar | 100 | Yes | - | User who last modified the record |
| ModifiedDate | datetime2 | - | Yes | GETDATE() | Last modification timestamp |

**Primary Key:** Id  
**Foreign Keys:**
- AuthorId → AspNetUsers.Id (CASCADE DELETE)
- CategoryId → Categories.Id (NO ACTION - category can be deleted independently)

**Unique Constraints:**
- Slug - Ensures unique URL slugs for SEO

**Indexes:**
- IX_BlogPosts_AuthorId
- IX_BlogPosts_CategoryId
- IX_BlogPosts_Slug (unique)
- IX_BlogPosts_PublicationDate
- IX_BlogPosts_IsPublished_PublicationDate (composite index for filtering published posts by date)

**Business Rules:**
- Slug must be unique across all posts
- Posts can be in draft (IsPublished = false) or published (IsPublished = true) state
- PublicationDate can be set in the future for scheduled publishing
- Posts can optionally be associated with a category

---

### LeadMagnets

Stores lead magnet configurations for lead capture forms and downloadable assets.

| Column Name | Data Type | Max Length | Nullable | Default | Description |
|------------|-----------|------------|----------|---------|-------------|
| Id | int | - | No | - | Primary Key (Identity, starts at 1) |
| CategoryId | int | - | No | - | Foreign Key to Categories.Id - Associated category |
| Title | nvarchar | 255 | No | - | Display title (e.g., "Free Budgeting Excel Template") |
| FormActionUrl | nvarchar | 500 | Yes | - | Endpoint URL for email marketing service where form submissions are sent |
| DownloadFileUrl | nvarchar | 500 | Yes | - | URL for downloadable asset (PDF, Excel, etc.) provided after signup |
| IsActive | bit | - | No | true | Whether the lead magnet is active |
| CreatedBy | nvarchar | 100 | Yes | - | User who created the record |
| CreatedDate | datetime2 | - | No | GETDATE() | Record creation timestamp |
| ModifiedBy | nvarchar | 100 | Yes | - | User who last modified the record |
| ModifiedDate | datetime2 | - | Yes | GETDATE() | Last modification timestamp |

**Primary Key:** Id  
**Foreign Keys:**
- CategoryId → Categories.Id (RESTRICT DELETE - prevents deletion if lead magnets exist)

**Indexes:**
- IX_LeadMagnets_CategoryId
- IX_LeadMagnets_CategoryId_IsActive (composite index for filtering active lead magnets by category)

**Business Rules:**
- Each lead magnet must be associated with a category
- Category cannot be deleted if it has associated lead magnets (RESTRICT)
- FormActionUrl points to the email marketing service endpoint
- DownloadFileUrl points to the downloadable asset provided to users

---

## Relationships

### Entity Relationship Diagram (Text Representation)

```
AspNetUsers (1) ──< (N) Categories
AspNetUsers (1) ──< (N) BlogPosts (AuthorId)
Categories (1) ──< (N) Categories (ParentCategoryId) [Self-referencing]
Categories (1) ──< (N) BlogPosts (CategoryId)
Categories (1) ──< (N) LeadMagnets
AspNetUsers (1) ──< (N) AspNetUserRoles ──> (N) AspNetRoles
AspNetUsers (1) ──< (N) AspNetUserClaims
AspNetUsers (1) ──< (N) AspNetUserLogins
AspNetUsers (1) ──< (N) AspNetUserTokens
AspNetUsers (1) ──< (N) AspNetUserPasskeys
AspNetRoles (1) ──< (N) AspNetRoleClaims
```

### Relationship Details

1. **AspNetUsers → Categories**
   - One user can have many categories
   - Delete behavior: CASCADE (deleting a user deletes their categories)

2. **AspNetUsers → BlogPosts**
   - One user can author many blog posts
   - Delete behavior: CASCADE (deleting a user deletes their posts)

3. **Categories → Categories (Self-referencing)**
   - One category can have many child categories
   - One category can have one parent category
   - Delete behavior: RESTRICT (cannot delete a category with children)

4. **Categories → BlogPosts**
   - One category can have many blog posts
   - Delete behavior: NO ACTION (category can be deleted independently)

5. **Categories → LeadMagnets**
   - One category can have many lead magnets
   - Delete behavior: RESTRICT (cannot delete a category with lead magnets)

6. **AspNetUsers ↔ AspNetRoles (Many-to-Many)**
   - Junction table: AspNetUserRoles
   - Delete behavior: CASCADE on both sides

---

## Indexes

### Performance Indexes

| Table | Index Name | Columns | Type | Unique | Filter |
|-------|------------|---------|------|--------|--------|
| AspNetUsers | UserNameIndex | NormalizedUserName | Unique | Yes | [NormalizedUserName] IS NOT NULL |
| AspNetUsers | EmailIndex | NormalizedEmail | Non-unique | No | - |
| AspNetRoles | RoleNameIndex | NormalizedName | Unique | Yes | [NormalizedName] IS NOT NULL |
| Categories | IX_Categories_UserId | UserId | Non-unique | No | - |
| Categories | IX_Categories_ParentCategoryId | ParentCategoryId | Non-unique | No | - |
| Categories | IX_Categories_UserId_Name | UserId, Name | Unique | Yes | - |
| BlogPosts | IX_BlogPosts_AuthorId | AuthorId | Non-unique | No | - |
| BlogPosts | IX_BlogPosts_CategoryId | CategoryId | Non-unique | No | - |
| BlogPosts | IX_BlogPosts_Slug | Slug | Unique | Yes | - |
| BlogPosts | IX_BlogPosts_PublicationDate | PublicationDate | Non-unique | No | - |
| BlogPosts | IX_BlogPosts_IsPublished_PublicationDate | IsPublished, PublicationDate | Composite | No | - |
| LeadMagnets | IX_LeadMagnets_CategoryId | CategoryId | Non-unique | No | - |
| LeadMagnets | IX_LeadMagnets_CategoryId_IsActive | CategoryId, IsActive | Composite | No | - |

### Foreign Key Indexes

All foreign key columns have indexes to optimize join operations:
- AspNetUserRoles: UserId, RoleId
- AspNetUserClaims: UserId
- AspNetRoleClaims: RoleId
- AspNetUserLogins: UserId
- AspNetUserPasskeys: UserId
- AspNetUserTokens: (implicit via composite primary key)

---

## Constraints

### Primary Key Constraints

All tables have primary keys:
- **Identity tables:** String-based (nvarchar(450)) using GUIDs
- **Application tables:** Integer-based (int) using Identity columns

### Foreign Key Constraints

All foreign keys are enforced with referential integrity:
- **CASCADE DELETE:** Used when child records should be deleted with parent (e.g., user deletion removes their categories)
- **RESTRICT DELETE:** Used when parent cannot be deleted if children exist (e.g., categories with child categories or lead magnets)
- **NO ACTION:** Used when relationship is optional and deletion should be independent (e.g., blog posts and categories)

### Unique Constraints

1. **AspNetUsers.NormalizedUserName** - Unique username (case-insensitive)
2. **AspNetRoles.NormalizedName** - Unique role name (case-insensitive)
3. **Categories (UserId, Name)** - Unique category name per user
4. **BlogPosts.Slug** - Unique URL slug for SEO

### Check Constraints

No explicit check constraints are defined. Business logic validation is handled in the application layer.

---

## Default Values

### Automatic Defaults (Database Level)

| Table | Column | Default Value | Description |
|-------|--------|---------------|-------------|
| AspNetUsers | CreatedDate | GETDATE() | Auto-set on insert |
| AspNetUsers | ModifiedDate | GETDATE() | Auto-set on insert/update |
| Categories | IsActive | true | New categories are active by default |
| Categories | CreatedDate | GETDATE() | Auto-set on insert |
| Categories | ModifiedDate | GETDATE() | Auto-set on insert/update |
| BlogPosts | IsPublished | false | New posts are drafts by default |
| BlogPosts | CreatedDate | GETDATE() | Auto-set on insert |
| BlogPosts | ModifiedDate | GETDATE() | Auto-set on insert/update |
| LeadMagnets | IsActive | true | New lead magnets are active by default |
| LeadMagnets | CreatedDate | GETDATE() | Auto-set on insert |
| LeadMagnets | ModifiedDate | GETDATE() | Auto-set on insert/update |

### Application-Level Defaults

- **AspNetUsers.IsActive:** false (must be explicitly set to true)
- **AspNetUsers.EmailConfirmed:** false (requires email verification)
- **AspNetUsers.PhoneNumberConfirmed:** false (requires phone verification)
- **AspNetUsers.TwoFactorEnabled:** false
- **AspNetUsers.LockoutEnabled:** false
- **AspNetUsers.AccessFailedCount:** 0

---

## Audit Trail

All application tables (Categories, BlogPosts, LeadMagnets) implement audit tracking through the following fields:

- **CreatedBy:** User identifier who created the record (nvarchar(100))
- **CreatedDate:** Timestamp when record was created (datetime2, default: GETDATE())
- **ModifiedBy:** User identifier who last modified the record (nvarchar(100))
- **ModifiedDate:** Timestamp when record was last modified (datetime2, default: GETDATE())

**Note:** The AspNetUsers table also includes audit fields, but they are required (not nullable) unlike the application tables where CreatedBy and ModifiedBy are nullable.

---

## Data Types Summary

| Data Type | Usage | Notes |
|-----------|-------|-------|
| nvarchar | Text fields | Unicode support, variable length |
| nvarchar(MAX) | Large text | For content fields (ContentBody, Data) |
| int | Identity columns | Auto-incrementing primary keys |
| bit | Boolean flags | IsActive, IsPublished, etc. |
| datetime2 | Timestamps | More precise than datetime |
| datetimeoffset | Lockout times | Timezone-aware |
| varbinary | Binary data | For passkey credentials |

---

## Migration History

1. **20251220231516_InitialCreate** - Created Identity tables (AspNetUsers, AspNetRoles, and related tables)
2. **20251228200140_AddCategoriesTable** - Added Categories table with hierarchical support
3. **20260123163210_AddPostTable** - Added BlogPosts table for blog functionality
4. **20260124165203_AddLeadMagnetTable** - Added LeadMagnets table for lead capture

---

## Notes

- All string fields use `nvarchar` for Unicode support
- Identity columns use SQL Server's IDENTITY feature (starts at 1, increments by 1)
- Default values are set at the database level using `GETDATE()` for timestamps
- Foreign key relationships enforce referential integrity
- Unique constraints prevent duplicate data at the database level
- Composite indexes are used for common query patterns (e.g., filtering published posts by date)

---

**Last Updated:** January 2025  
**Schema Version:** 4 (based on migration count)

