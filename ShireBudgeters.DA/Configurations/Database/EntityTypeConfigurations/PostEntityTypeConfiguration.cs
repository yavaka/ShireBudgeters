using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.DA.Configurations.Database.EntityTypeConfigurations;

internal class PostEntityTypeConfiguration : IEntityTypeConfiguration<PostModel>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<PostModel> builder)
    {
        // Configure table name
        builder.ToTable("BlogPosts");

        // Configure primary key
        builder.HasKey(e => e.Id);

        // Configure properties
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.AuthorId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Slug)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.ContentBody)
            .HasColumnType("nvarchar(MAX)");

        builder.Property(e => e.FeaturedImageUrl)
            .HasMaxLength(500);

        builder.Property(e => e.PublicationDate)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(e => e.IsPublished)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.MetaDescription)
            .HasMaxLength(300);

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
        builder.HasOne(e => e.Author)
            .WithMany()
            .HasForeignKey(e => e.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure indexes
        builder.HasIndex(e => e.AuthorId)
            .HasDatabaseName("IX_BlogPosts_AuthorId");

        builder.HasIndex(e => e.CategoryId)
            .HasDatabaseName("IX_BlogPosts_CategoryId");

        builder.HasIndex(e => e.Slug)
            .IsUnique()
            .HasDatabaseName("IX_BlogPosts_Slug");

        builder.HasIndex(e => e.PublicationDate)
            .HasDatabaseName("IX_BlogPosts_PublicationDate");

        builder.HasIndex(e => new { e.IsPublished, e.PublicationDate })
            .HasDatabaseName("IX_BlogPosts_IsPublished_PublicationDate");
    }
}

