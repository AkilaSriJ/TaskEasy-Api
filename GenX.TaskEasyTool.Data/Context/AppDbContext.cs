using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenX.TaskEasyTool.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GenX.TaskEasyTool.Data.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Epic> Epics { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<EpicUser> EpicUsers { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<SprintUser> SprintUsers { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<WorkTask> Tasks { get; set; }
        public DbSet<WorkTaskUser> WorkTaskUsers { get; set; }
        public DbSet<WorkTaskLog> WorkTaskLogs { get; set; }
        public DbSet<ProjectLog> ProjectLogs { get; set; }
        public DbSet<SprintLog> SprintLogs { get; set; }
        public DbSet<EpicLog> EpicLogs { get; set; }
        public DbSet<WorkTaskLabel> WorkTaskLabels { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>().HasKey(p => p.Id);

            modelBuilder.Entity<Project>()
               .HasOne(p => p.CreatedByUser)
               .WithMany(u => u.Projects)
               .HasForeignKey(p => p.CreatedByUserId)
               .OnDelete(DeleteBehavior.Restrict);

            // One Project → Many Epics
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Epics)
                .WithOne(e => e.Project)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Many-to-Many: Project ↔ Users (via ProjectUser)
            modelBuilder.Entity<ProjectUser>()
                .HasKey(pu => new { pu.ProjectId, pu.UserId });

            modelBuilder.Entity<ProjectUser>()
                .HasOne(pu => pu.Project)
                .WithMany(p => p.ProjectUsers)
                .HasForeignKey(pu => pu.ProjectId);

            modelBuilder.Entity<ProjectUser>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.ProjectUsers)
                .HasForeignKey(pu => pu.UserId);

            modelBuilder.Entity<Label>()
                .HasIndex(l => l.Name)
                .IsUnique();


            // Many-to-Many: Epic ↔ Users (via EpicUser)
            modelBuilder.Entity<Epic>()
                .HasOne(e => e.Project)
                .WithMany(p => p.Epics)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EpicUser>()
                .HasKey(eu => new { eu.EpicId, eu.UserId });

            modelBuilder.Entity<EpicUser>()
                .HasOne(eu => eu.Epic)
                .WithMany(e => e.EpicUsers)
                .HasForeignKey(eu => eu.EpicId);

            modelBuilder.Entity<EpicUser>()
                .HasOne(eu => eu.User)
                .WithMany(u => u.EpicUsers)
                .HasForeignKey(eu => eu.UserId);

            modelBuilder.Entity<SprintUser>()
                .HasKey(su => new { su.SprintId, su.UserId });

            modelBuilder.Entity<SprintUser>()
                .HasOne(su => su.Sprint)
                .WithMany(s => s.SprintUsers)
                .HasForeignKey(su => su.SprintId);

            modelBuilder.Entity<SprintUser>()
                .HasOne(su => su.User)
                .WithMany(u => u.SprintUsers)
                .HasForeignKey(su => su.UserId);

            // Configure CreatedByUser navigation
            modelBuilder.Entity<Sprint>()
                .HasOne(s => s.CreatedByUser)
                .WithMany()
                .HasForeignKey(s => s.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WorkTask>().ToTable("Tasks");

            modelBuilder.Entity<WorkTaskUser>()
                .HasKey(wtu => new { wtu.WorkTaskId, wtu.UserId });

            modelBuilder.Entity<WorkTaskUser>()
                .HasOne(wtu => wtu.WorkTask)
                .WithMany(wt => wt.WorkTaskUsers)
                .HasForeignKey(wtu => wtu.WorkTaskId);

            modelBuilder.Entity<WorkTaskUser>()
                .HasOne(wtu => wtu.User) 
                .WithMany(u => u.WorkTaskUsers)
                .HasForeignKey(wtu => wtu.UserId);
            modelBuilder.Entity<WorkTask>()
                .Property(t => t.Status)
                .HasConversion<string>();

            modelBuilder.Entity<WorkTaskLabel>()
                .HasKey(wtl=>new {wtl.WorkTaskId,wtl.LabelId});

            modelBuilder.Entity<WorkTaskLabel>()
                .HasOne(wtl=>wtl.WorkTask)
                .WithMany(wt=>wt.WorkTaskLabels)
                .HasForeignKey(wtl=>wtl.WorkTaskId);

            modelBuilder.Entity<WorkTaskLabel>()
                .HasOne(wtl=>wtl.Label)
                .WithMany(l=>l.workTaskLabels)
                .HasForeignKey(wtl=>wtl.LabelId);
        }
    }
}

