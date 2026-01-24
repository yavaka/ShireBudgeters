using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ShireBudgeters.Common.Common.Constants;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.DA.Configurations.Database;

/// <summary>
/// Seeds the database with initial data for development and testing.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DatabaseSeeder"/> class.
/// </remarks>
/// <param name="context">The database context.</param>
/// <param name="userManager">The user manager.</param>
/// <param name="roleManager">The role manager.</param>
/// <param name="configuration">The configuration.</param>
/// <param name="logger">The logger.</param>
public class DatabaseSeeder(
    ShireBudgetersDbContext context,
    UserManager<UserModel> userManager,
    IConfiguration configuration,
    ILogger<DatabaseSeeder> logger)
{
    private readonly ShireBudgetersDbContext _context = context;
    private readonly UserManager<UserModel> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<DatabaseSeeder> _logger = logger;

    /// <summary>
    /// Seeds the database with initial data.
    /// </summary>
    /// <param name="seedSampleData">Whether to seed sample data (categories, posts, lead magnets).</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SeedAsync(bool seedSampleData = false)
    {
        try
        {
            _logger.LogInformation("Starting database seeding...");

            await SeedAdminUserAsync();
            
            if (seedSampleData)
            {
                await SeedSampleDataAsync();
            }

            _logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    /// <summary>
    /// Seeds the admin user.
    /// </summary>
    private async Task SeedAdminUserAsync()
    {
        _logger.LogInformation("Seeding admin user...");

        var adminEmail = _configuration["DatabaseSeeder:AdminEmail"] ?? "admin@shirebudgeters.com";
        var adminPassword = _configuration["DatabaseSeeder:AdminPassword"] ?? "Admin@123456";
        var adminFirstName = _configuration["DatabaseSeeder:AdminFirstName"] ?? "Admin";
        var adminLastName = _configuration["DatabaseSeeder:AdminLastName"] ?? "User";

        var existingUser = await _userManager.FindByEmailAsync(adminEmail);
        if (existingUser != null)
        {
            _logger.LogDebug("Admin user with email {Email} already exists, skipping.", adminEmail);
            return;
        }

        var adminUser = new UserModel
        {
            UserName = adminEmail,
            Email = adminEmail,
            NormalizedUserName = adminEmail.ToUpperInvariant(),
            NormalizedEmail = adminEmail.ToUpperInvariant(),
            EmailConfirmed = true,
            FirstName = adminFirstName,
            LastName = adminLastName,
            IsActive = true,
            CreatedBy = "System",
            ModifiedBy = "System"
        };

        var result = await _userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            _logger.LogInformation("Created admin user: {Email}", adminEmail);
        }
        else
        {
            _logger.LogError("Failed to create admin user. Errors: {Errors}",
                string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

    /// <summary>
    /// Seeds sample data for development and testing.
    /// </summary>
    private async Task SeedSampleDataAsync()
    {
        _logger.LogInformation("Seeding sample data...");

        var adminUser = await _userManager.FindByEmailAsync(
            _configuration["DatabaseSeeder:AdminEmail"] ?? "admin@shirebudgeters.com");

        if (adminUser == null)
        {
            _logger.LogWarning("Admin user not found. Cannot seed sample data.");
            return;
        }

        await SeedSampleCategoriesAsync(adminUser.Id);
        await SeedSamplePostsAsync(adminUser.Id);
    }

    /// <summary>
    /// Seeds sample categories.
    /// </summary>
    private async Task SeedSampleCategoriesAsync(string userId)
    {
        _logger.LogInformation("Seeding sample categories...");

        var categories = new List<CategoryModel>
        {
            new()
            {
                Name = "Budgeting",
                Description = "Tips and strategies for effective budgeting",
                Color = "#4CAF50",
                UserId = userId,
                IsActive = true,
                CreatedBy = userId,
                ModifiedBy = userId
            },
            new()
            {
                Name = "Saving Money",
                Description = "Ways to save money and build wealth",
                Color = "#2196F3",
                UserId = userId,
                IsActive = true,
                CreatedBy = userId,
                ModifiedBy = userId
            },
            new()
            {
                Name = "Investing",
                Description = "Investment strategies and advice",
                Color = "#FF9800",
                UserId = userId,
                IsActive = true,
                CreatedBy = userId,
                ModifiedBy = userId
            }
        };

        foreach (var category in categories)
        {
            var exists = await _context.Categories
                .AnyAsync(c => c.UserId == userId && c.Name == category.Name);

            if (!exists)
            {
                await _context.Categories.AddAsync(category);
                _logger.LogDebug("Created sample category: {CategoryName}", category.Name);
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Sample categories seeded.");
    }

    /// <summary>
    /// Seeds sample blog posts.
    /// </summary>
    private async Task SeedSamplePostsAsync(string userId)
    {
        _logger.LogInformation("Seeding sample blog posts...");

        var categories = await _context.Categories
            .Where(c => c.UserId == userId)
            .ToListAsync();

        if (!categories.Any())
        {
            _logger.LogWarning("No categories found. Cannot seed sample posts.");
            return;
        }

        var budgetingCategory = categories.FirstOrDefault(c => c.Name == "Budgeting");

        var posts = new List<PostModel>
        {
            new()
            {
                AuthorId = userId,
                Title = "Getting Started with Budgeting",
                Slug = "getting-started-with-budgeting",
                ContentBody = "<h1>Getting Started with Budgeting</h1><p>Budgeting is the foundation of financial success. In this post, we'll explore the basics of creating and maintaining a budget.</p>",
                MetaDescription = "Learn the fundamentals of budgeting and take control of your finances.",
                PublicationDate = DateTime.UtcNow.AddDays(-7),
                IsPublished = true,
                CategoryId = budgetingCategory?.Id,
                CreatedBy = userId,
                ModifiedBy = userId
            },
            new()
            {
                AuthorId = userId,
                Title = "10 Money-Saving Tips for 2025",
                Slug = "10-money-saving-tips-2025",
                ContentBody = "<h1>10 Money-Saving Tips for 2025</h1><p>Discover practical strategies to save money and improve your financial health.</p>",
                MetaDescription = "Practical money-saving tips to help you build wealth in 2025.",
                PublicationDate = DateTime.UtcNow.AddDays(-3),
                IsPublished = true,
                CategoryId = categories.FirstOrDefault(c => c.Name == "Saving Money")?.Id,
                CreatedBy = userId,
                ModifiedBy = userId
            }
        };

        foreach (var post in posts)
        {
            var exists = await _context.BlogPosts
                .AnyAsync(p => p.Slug == post.Slug);

            if (!exists)
            {
                await _context.BlogPosts.AddAsync(post);
                _logger.LogDebug("Created sample post: {PostTitle}", post.Title);
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Sample blog posts seeded.");
    }
}

