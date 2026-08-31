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
        
        // Add any model configurations here if needed
        // Example: modelBuilder.Entity<Application>().HasKey(a => a.Id);
    }
}
