using LogAnalizerServer.Models;
using Microsoft.EntityFrameworkCore;

namespace LogAnalizerServer.Data;

public class LogAnalizerServerDbContext : DbContext
{
    public LogAnalizerServerDbContext(DbContextOptions<LogAnalizerServerDbContext> options)
         : base(options)
    {
    }

    public DbSet<AlarmLog> AlarmLogs { get; set; } = null!;
    public DbSet<ComparisonResult> ComparisonResults { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AlarmLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            
            entity.Property(e => e.Id)
                .UseIdentityColumn(); 

            entity.Property(e => e.TimeWhenLogged).IsRequired();
            entity.Property(e => e.LocalZoneTime).IsRequired();

            entity.Property(e => e.SequenceNumber).IsRequired();
            entity.Property(e => e.AlarmId).IsRequired().HasMaxLength(255);
            entity.Property(e => e.AlarmClass).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Resource).IsRequired().HasMaxLength(255);
            entity.Property(e => e.LoggedBy).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Reference).HasMaxLength(255);
            entity.Property(e => e.PrevState).IsRequired().HasMaxLength(255);
            entity.Property(e => e.LogAction).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FinalState).IsRequired().HasMaxLength(255);
            entity.Property(e => e.AlarmMessage).IsRequired().HasMaxLength(500);
            entity.Property(e => e.GenerationTime).IsRequired();
            entity.Property(e => e.GenerationTimeUtc).IsRequired();
            entity.Property(e => e.Project).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<ComparisonResult>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AlarmMessage).IsRequired().HasMaxLength(500);
        });
    }

 
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=LogAnalizerTestDb;Trusted_Connection=True;");
        }
        
    }
    
    
    
    
}