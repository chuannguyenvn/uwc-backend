using Microsoft.EntityFrameworkCore;
using Commons.Models;
using Commons.Types;

namespace Repositories.Managers;

public class UwcDbContext : DbContext, ISeedable
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Message>()
            .HasOne(message => message.SenderAccount)
            .WithMany()
            .HasForeignKey(message => message.SenderAccountId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Message>()
            .HasOne(message => message.ReceiverAccount)
            .WithMany()
            .HasForeignKey(message => message.ReceiverAccountId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<TaskData>()
            .HasOne(taskData => taskData.AssignerAccount)
            .WithMany()
            .HasForeignKey(taskData => taskData.AssignerAccountId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<TaskData>()
            .HasOne(taskData => taskData.AssigneeAccount)
            .WithMany()
            .HasForeignKey(taskData => taskData.AssigneeAccountId)
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

    public void AddAccount(Account entry)
    {
        AccountTable.Add(entry);
    }

    public void AddUserProfile(UserProfile entry)
    {
        UserProfileTable.Add(entry);
    }

    public void AddMcpData(McpData entry)
    {
        McpDataTable.Add(entry);
    }

    public void AddMcpEmptyRecord(McpEmptyRecord entry)
    {
        McpEmptyRecordTable.Add(entry);
    }

    public void AddMcpFillLevelLog(McpFillLevelLog entry)
    {
        McpFillLevelLogTable.Add(entry);
    }

    public void AddVehicleData(VehicleData entry)
    {
        VehicleDataTable.Add(entry);
    }

    public void AddTaskData(TaskData entry)
    {
        TaskDataTable.Add(entry);
    }

    public void AddMessage(Message entry)
    {
        MessageTable.Add(entry);
    }

    public void Complete()
    {
        SaveChanges();
    }
}