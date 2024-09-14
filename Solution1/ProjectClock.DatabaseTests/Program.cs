using Microsoft.EntityFrameworkCore;
using ProjectClock.BusinessLogic.Dtos.Organization;
using ProjectClock.BusinessLogic.Services.OrganizationServices;
using ProjectClock.BusinessLogic.Services.UserServices;
using ProjectClock.BusinessLogic.Services.WorkingTimeServices;
using ProjectClock.Database;
using ProjectClock.Database.Entities;

namespace ProjectClock.DatabaseTests
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjectClockDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ProjectClock;Trusted_Connection=True;");

            // Inicjalizacja DbContext z opcjami
            using (var dbContext = new ProjectClockDbContext(optionsBuilder.Options))
            {
                UserServices userServices = new UserServices(dbContext);
                //ProjectServices projectServices = new ProjectServices(dbContext);
                OrganizationServices organizationService = new OrganizationServices(dbContext);



                User user = new User("Zdzichu", "Po Kielichu", "zdzszek@kieliszek");
                await userServices.Create(user);

                Project newVodka = new Project() { Name = "Zytnia 70%", };
                Organization polmos = new Organization() { Name = "Polmos", Projects = new List<Project>() { newVodka } };

                //await userServices.SignUserToOrganization(user, polmos);
                OrganizationUser oU = new OrganizationUser() { User = user, Organization = polmos };
                await organizationService.Create(polmos);

                Console.WriteLine("End");
            }

        }
    }
}
