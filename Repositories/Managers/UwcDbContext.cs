using Microsoft.EntityFrameworkCore;
using Commons.Models;
using Commons.Types;

namespace Repositories.Managers;

public class UwcDbContext : DbContext
{
    public UwcDbContext(DbContextOptions<UwcDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<McpData> McpDatas { get; set; }
    public DbSet<VehicleData> VehicleDatas { get; set; }
    public DbSet<TaskData> TaskDatas { get; set; }
    public DbSet<Message> Messages { get; set; }

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
    }
}