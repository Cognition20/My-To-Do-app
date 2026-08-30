using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using To_Do.DataAccess.Models;

namespace To_Do.DataAccess.ApplicationDbContext.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(t => t.Login)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(t => t.Email)
            .IsRequired()
            .HasMaxLength(96);

        builder.Property(t => t.PasswordHash)
            .IsRequired();
        
        builder.HasIndex(t => t.Login).IsUnique();
        builder.HasIndex(t => t.Email).IsUnique();
    }
}