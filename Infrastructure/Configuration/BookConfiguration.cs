using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

internal class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder
            .HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(b => b.Author)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.ISBN)
            .IsRequired()
            .HasMaxLength(13);

        builder.HasIndex(b => b.ISBN)
            .IsUnique();
    }
}
