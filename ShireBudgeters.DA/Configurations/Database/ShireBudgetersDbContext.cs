using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShireBudgeters.DA.Configurations.Database.EntityTypeConfigurations;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.DA.Configurations.Database;

/// <summary>
/// The database context for the Shire Budgeters application.
/// </summary>
public class ShireBudgetersDbContext(DbContextOptions<ShireBudgetersDbContext> options) : IdentityDbContext<UserModel>(options)
{
    public DbSet<CategoryModel> Categories { get; set; }

    public DbSet<PostModel> BlogPosts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<UserModel>());
        new CategoryEntityTypeConfiguration().Configure(modelBuilder.Entity<CategoryModel>());
        new PostEntityTypeConfiguration().Configure(modelBuilder.Entity<PostModel>());
    }
}
