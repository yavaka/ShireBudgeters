using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShireBudgeters.DA.Models;

namespace ShireBudgeters.DA.Configurations.Database.EntityTypeConfigurations;

internal class CommentEntityTypeConfiguration : IEntityTypeConfiguration<CommentModel>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<CommentModel> builder)
    {
        builder.ToTable("PostComments");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.AuthorName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.AuthorEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.ContentBody)
            .HasColumnType("nvarchar(MAX)");

        builder.Property(e => e.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(e => e.Post)
            .WithMany()
            .HasForeignKey(e => e.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.PostId)
            .HasDatabaseName("IX_PostComments_PostId");
    }
}
