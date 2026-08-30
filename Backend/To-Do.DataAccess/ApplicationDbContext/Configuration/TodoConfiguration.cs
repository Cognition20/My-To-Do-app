using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using To_Do.DataAccess.Models;

namespace To_Do.DataAccess.ApplicationDbContext.Configuration;

public class TodoConfiguration : IEntityTypeConfiguration<ToDo>
{
    public void Configure(EntityTypeBuilder<ToDo> builder)
    {
        builder.ToTable("ToDos");
        
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(t => t.Description)
            .HasMaxLength(200);
        
        builder.Property(t => t.CreatedAtUtc)
            .IsRequired();
        
        builder.Property(t => t.UpdatedAtUtc);
        
        builder.Property(t => t.IsCompleted)
            .IsRequired();

        builder.HasIndex(t => t.UserId);
        builder.HasIndex(t => t.CategoryId);
        
        builder.HasOne(t => t.User)
            .WithMany(u => u.ToDos)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(t => t.Category)
            .WithMany(c => c.ToDos)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.ClientSetNull);;
        
    }
}