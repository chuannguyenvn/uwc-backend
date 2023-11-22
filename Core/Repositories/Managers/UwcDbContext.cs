using Microsoft.EntityFrameworkCore;
using Commons.Models;
using Commons.Types;

namespace Repositories.Managers;

public class UwcDbContext : DbContext
{
    public UwcDbContext(DbContextOptions<UwcDbContext> options) : base(options)
    {
    }

    public DbSet<Account> AccountTable { get; set; }
    public DbSet<UserProfile> UserProfileTable { get; set; }
    public DbSet<McpData> McpDataTable { get; set; }
    public DbSet<McpEmptyRecord> McpEmptyRecordTable { get; set; }
    public DbSet<McpFillLevelLog> McpFillLevelLogTable { get; set; }
    public DbSet<VehicleData> VehicleDataTable { get; set; }
    public DbSet<TaskData> TaskDataTable { get; set; }
    public DbSet<Message> MessageTable { get; set; }
    public DbSet<Setting> SettingTable { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Message>()
            .HasOne(message => message.SenderUserProfile)
            .WithMany()
            .HasForeignKey(message => message.SenderProfileId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Message>()
            .HasOne(message => message.ReceiverUserProfile)
            .WithMany()
            .HasForeignKey(message => message.ReceiverProfileId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<TaskData>()
            .HasOne(taskData => taskData.AssignerProfile)
            .WithMany()
            .HasForeignKey(taskData => taskData.AssignerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<TaskData>()
            .HasOne(taskData => taskData.AssigneeProfile)
            .WithMany()
            .HasForeignKey(taskData => taskData.AssigneeId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Coordinate>()
            .HasNoKey();

        modelBuilder.Entity<Account>()
            .HasOne<UserProfile>(account => account.UserProfile)
            .WithOne(profile => profile.Account)
            .HasForeignKey<UserProfile>(profile => profile.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<McpData>()
            .HasMany(mcpData => mcpData.McpFillLevelLogs)
            .WithOne(log => log.McpData)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<McpData>()
            .HasMany(mcpData => mcpData.McpEmptyRecords)
            .WithOne(log => log.McpData)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<McpData>()
            .Ignore(data => data.Coordinate);
    }
}