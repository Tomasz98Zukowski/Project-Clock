using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ProjectClock.Database.Extensions
{
    public static class ServiceColletionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProjectClockDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("ProjectClock")));
        }
    }
}
