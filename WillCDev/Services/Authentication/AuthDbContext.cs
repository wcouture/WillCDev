using Microsoft.EntityFrameworkCore;
using Shared.Models.Authentication;

namespace WillCDev.Services;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().Property(u => u.Username).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.PasswordHash).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.Email).IsRequired(false);
    }
}
