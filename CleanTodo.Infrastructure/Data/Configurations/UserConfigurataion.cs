using CleanTodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Username)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Password)
            .IsRequired()
            .HasMaxLength(200);
    }
}
