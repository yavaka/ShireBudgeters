using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.DA.Configurations.Database.EntityTypeConfigurations;

internal class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryModel>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<CategoryModel> builder)
    {
        // Configure table name
        builder.ToTable("Categories");

        // Configure primary key
        builder.HasKey(e => e.Id);

        // Configure properties
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Slug)
            .HasMaxLength(120);

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.Color)
            .HasMaxLength(50);

        builder.Property(e => e.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(100);

        builder.Property(e => e.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(e => e.ModifiedBy)
            .HasMaxLength(100);

        builder.Property(e => e.ModifiedDate)
            .HasDefaultValueSql("GETDATE()");

        // Configure relationships
        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.ParentCategory)
            .WithMany(e => e.ChildCategories)
            .HasForeignKey(e => e.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure indexes
        builder.HasIndex(e => e.UserId)
            .HasDatabaseName("IX_Categories_UserId");

        builder.HasIndex(e => e.ParentCategoryId)
            .HasDatabaseName("IX_Categories_ParentCategoryId");

        builder.HasIndex(e => e.Slug)
            .IsUnique()
            .HasDatabaseName("IX_Categories_Slug")
            .HasFilter("[Slug] IS NOT NULL");

        builder.HasIndex(e => new { e.UserId, e.Name })
            .IsUnique()
            .HasDatabaseName("IX_Categories_UserId_Name");
    }
}
