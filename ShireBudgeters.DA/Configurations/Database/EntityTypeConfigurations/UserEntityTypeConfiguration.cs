using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.DA.Configurations.Database.EntityTypeConfigurations;

/// <summary>
/// Entity type configuration for the UserModel.
/// </summary>
internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserModel>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        // Configure the table name and properties of the entity   
        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.IsActive)
            .IsRequired();

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(e => e.ModifiedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.ModifiedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");
    }
}
