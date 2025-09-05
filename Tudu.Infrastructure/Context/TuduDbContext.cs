using Microsoft.EntityFrameworkCore;
using Tudu.Domain.Entities;

namespace Tudu.Infrastructure.Context;

public class TuduDbContext : DbContext
    {
    public TuduDbContext(DbContextOptions<TuduDbContext> options)
        : base(options)
        {
        //Database.EnsureCreated();
        }

    public DbSet<User> Users { get; set; }
    public DbSet<UserTask> UserTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        base.OnModelCreating(modelBuilder);

        // User Configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.UserName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Password).IsRequired();
            entity.Property(u => u.Email).IsRequired();

            entity.Property(u => u.CreatedAt).IsRequired();
        });

        // Task Configuration
        modelBuilder.Entity<UserTask>(entity =>
        {
            // Relationships
            entity.HasOne(t => t.User)
                  .WithMany(u => u.Tasks)
                  .HasForeignKey(t => t.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        }
    }

