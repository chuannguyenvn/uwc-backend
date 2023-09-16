using Microsoft.EntityFrameworkCore;
using Commons.Models;

namespace Repositories.Managers;

public class UwcDbContext : DbContext
{
    public UwcDbContext(DbContextOptions<UwcDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<McpData> McpData { get; set; }
    public DbSet<VehicleData> VehicleData { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}