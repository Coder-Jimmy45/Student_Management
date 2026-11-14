using Microsoft.EntityFrameworkCore;
using Student_Management.Models;

namespace Student_Management.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Student> Students => Set<Student>();
    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Students", table => table.HasCheckConstraint("CK_Students_Age", "[Age] BETWEEN 5 AND 120"));
            entity.Property(student => student.Name).HasMaxLength(100).IsRequired();
            entity.Property(student => student.Email).HasMaxLength(256).IsRequired();
            entity.Property(student => student.Course).HasMaxLength(100).IsRequired();
            entity.Property(student => student.CreatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(student => student.RowVersion).IsRowVersion();
            entity.HasIndex(student => student.Email).IsUnique();
        });

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("Users");
            entity.Property(user => user.Username).HasMaxLength(100).IsRequired();
            entity.Property(user => user.PasswordHash).HasMaxLength(512).IsRequired();
            entity.Property(user => user.Role).HasMaxLength(50).IsRequired();
            entity.Property(user => user.CreatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(user => user.Username).IsUnique();
        });
    }
}
