using Microsoft.EntityFrameworkCore;
using ShireBudgeters.DA.Configurations.Database.EntityTypeConfigurations;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.DA.Configurations.Database;

/// <summary>
/// The database context for the Shire Budgeters application.
/// </summary>
public class ShireBudgetersDbContext(DbContextOptions<ShireBudgetersDbContext> options) : DbContext(options)
{
    public DbSet<UserModel> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<UserModel>());
    }
}
