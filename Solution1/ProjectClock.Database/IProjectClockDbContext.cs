using Microsoft.EntityFrameworkCore;
using ProjectClock.Database.Entities;

namespace ProjectClock.Database
{
    public interface IProjectClockDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<Organization> Organizations { get; set; }
        DbSet<OrganizationUser> OrganizationsUsers { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<UserProject> UserProjects { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<WorkingTime> WorkingTimes { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}