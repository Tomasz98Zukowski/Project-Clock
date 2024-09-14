using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectClock.Database
{
    public class ProjectClockDbContextFactory : IDesignTimeDbContextFactory<ProjectClockDbContext>
    {
        public ProjectClockDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjectClockDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ProjectClock;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new ProjectClockDbContext(optionsBuilder.Options);
        }
    }
}