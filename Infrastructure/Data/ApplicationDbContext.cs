using Core.Entities;
using Core.Entities.TimesheetReminder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    // Infrastructure/Data/ApplicationDbContext.cs
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
                
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<JobSchedule> JobSchedules { get; set; }
        public DbSet<JobRunLog> JobRunLogs { get; set; }        
        public DbSet<ExecutionLog> ExecutionLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationship between JobRunLog and JobSchedule
            modelBuilder.Entity<JobRunLog>()
                .HasOne(jr => jr.JobSchedule)
                .WithMany() // Or with navigation property if you need bidirectional navigation
                .HasForeignKey(jr => jr.JobSchedule_JobId);

            base.OnModelCreating(modelBuilder);
        }
    }

}
