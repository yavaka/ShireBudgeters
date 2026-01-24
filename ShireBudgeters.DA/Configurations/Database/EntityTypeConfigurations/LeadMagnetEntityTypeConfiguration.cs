using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.DA.Configurations.Database.EntityTypeConfigurations;

internal class LeadMagnetEntityTypeConfiguration : IEntityTypeConfiguration<LeadMagnetModel>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<LeadMagnetModel> builder)
    {
        // Configure table name
        builder.ToTable("LeadMagnets");

        // Configure primary key
        builder.HasKey(e => e.Id);

        // Configure properties
        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.CategoryId)
            .IsRequired();

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.FormActionUrl)
            .HasMaxLength(500);

        builder.Property(e => e.DownloadFileUrl)
            .HasMaxLength(500);

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
        builder.HasOne(e => e.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure indexes
        builder.HasIndex(e => e.CategoryId)
            .HasDatabaseName("IX_LeadMagnets_CategoryId");

        builder.HasIndex(e => new { e.CategoryId, e.IsActive })
            .HasDatabaseName("IX_LeadMagnets_CategoryId_IsActive");
    }
}

