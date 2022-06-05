using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HighRiskDiseaseSurvellance.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HighRiskDiseaseSurvellance.Persistence
{
    public partial class SurveillanceContext : DbContext
    {
        const string SelectNextSequenceIdSql = "SELECT nextval('{0}');";

        public SurveillanceContext()
        {
        }
        public SurveillanceContext(DbContextOptions<SurveillanceContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<SurveillanceRecord> Records { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OfficeUser> OfficeUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var username     = Environment.GetEnvironmentVariable("MYSQL_USERNAME");
                var password     = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");
                var addressParts = Environment.GetEnvironmentVariable("MYSQL_ADDRESS")?.Split(':');
                var host         = addressParts?[0];
                var port         = addressParts?[1];
                var database     = Environment.GetEnvironmentVariable("MYSQL_DATABASE") ?? "aspnet_demo";
                var version      = Environment.GetEnvironmentVariable("MYSQL_VERSION")  ?? "5.7.18-mysql";
                var connstr      = $"server={host};port={port};user={username};password={password};database={database}";
                optionsBuilder.UseMySql(connstr, ServerVersion.Parse(version))
                              .LogTo(Console.WriteLine, LogLevel.Information)
                              .EnableSensitiveDataLogging()
                              .EnableDetailedErrors();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                        .HasCharSet("utf8");
            modelBuilder.Entity<User>(b =>
                                      {
                                          b.HasIndex(u => u.PhoneNumber);
                                          b.HasIndex(u => u.WeChatOpenId);
                                      });

            modelBuilder.Entity<SurveillanceRecord>(s =>
                                                    {
                                                        s.HasIndex(r => r.UserId);
                                                        s.OwnsOne(r => r.UserInfo);
                                                    });

            modelBuilder.Entity<Order>(o =>
                                       {
                                           o.HasIndex(od => od.UserId);
                                           o.OwnsOne(od => od.UserInfo);
                                       });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateEntitiesModification();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected void UpdateEntitiesModification()
        {
            var entries = ChangeTracker.Entries().ToArray();
            foreach (var e in entries)
            {
               
                if (e.Entity is AppAggregateRoot updateable)
                {
                    if (e.State == EntityState.Modified)
                    {
                        updateable.UpdateModification();
                    }
                    else if (e.State == EntityState.Added)
                    {
                        updateable.UpdateCreation();
                    }
                }
            }
        }
    }
}