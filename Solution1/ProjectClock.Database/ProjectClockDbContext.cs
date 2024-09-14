using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProjectClock.Database.Entities;

namespace ProjectClock.Database
{
    public class ProjectClockDbContext : DbContext
    {

        public ProjectClockDbContext(DbContextOptions<ProjectClockDbContext> options) : base(options)
        {

        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<WorkingTime> WorkingTimes { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<OrganizationUser> OrganizationsUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkingTime>(eb =>
            {
                eb.HasOne(wt => wt.User);
                eb.HasOne(wt => wt.Project);
            });
                
                

            modelBuilder.Entity<User>(eb =>
            {
                eb.HasMany(u => u.WorkingTimes)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId);

            });


            modelBuilder.Entity<Project>(eb =>
            {
                eb.HasMany(u => u.WorkingTimes)
                .WithOne(u => u.Project)
                .HasForeignKey(u => u.ProjectId);

            });

            modelBuilder.Entity<Account>(eb =>
            {
                eb.HasOne(a => a.User);             
            });

            modelBuilder.Entity<OrganizationUser>().HasKey(x => new { x.UserId, x.OrganizationId });
           
        }
    }
}

