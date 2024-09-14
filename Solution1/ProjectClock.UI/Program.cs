
using Newtonsoft.Json;
using ProjectClock.UI.Menu;
using ProjectClock.Database;
using ProjectClock.BusinessLogic.SqlServices;
using ProjectClock.Database.Entities;
using System.Data.SqlTypes;
using ProjectClock.BusinessLogic.SqlServices.SqlWorkingTimeServices;
using ProjectClock.BusinessLogic.SqlServices.SqlUserServices;
using Microsoft.EntityFrameworkCore;
using ProjectClock.BusinessLogic.SqlServices.SqlProjectServices;
using System.ComponentModel.DataAnnotations;


namespace ProjectClock.UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //MainMenu.RunMenu();

            var optionsBuilder = new DbContextOptionsBuilder<ProjectClockDbContext>();
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-0D8TD9S;Database=ProjectClockDb;Integrated Security=True;TrustServerCertificate=True"); // Zastąp to swoim połączeniem do bazy danych

            // Inicjalizacja DbContext z opcjami
            using (var dbContext = new ProjectClockDbContext(optionsBuilder.Options))
            {
                //User user = new User("Barack", "Obama", "barack@whitehouse.com");
                //Project project = new Project() { Name = "Retirement" };
                //WorkingTime workingTime = new WorkingTime() { Project = project, User = user, };


                SqlUserGeneral sqlUserGeneral = new SqlUserGeneral(dbContext);
                SqlUserGetter sqlUserGetter = new SqlUserGetter(dbContext, sqlUserGeneral);
                SqlWorkingTime sqlWorkingTime = new SqlWorkingTime(dbContext);
                SqlUserCreator sqlUserCreator = new SqlUserCreator(dbContext, sqlUserGeneral);
                SqlProjectCreator sqlProjectCreator = new SqlProjectCreator(dbContext);

                //sqlWorkingTime.PushToSql(workingTime);

                //sqlWorkingTime.StartWork(1);
                ////sqlWorkingTime.StopWork(1);

                //User user2 = new User("John", "Lenon", "john@wp.pl");
                //sqlUserCreator.Create(user2);
                //sqlUserCreator.Create("Keith", "Flint", "keith@wp.pl");

                //Project project2 = new Project() { Name = "The Beatles" };
                //sqlProjectCreator.Create(project2.Name);


                User user3 = new User("Maxim3", "JustMaxim3", "maxim3@wp.pl");
                sqlUserCreator.Create(user3);
                Project project = new Project() { Name = "Solo Album" };
                sqlProjectCreator.Create(project.Name);
                user3.Projects.Add(project);
                dbContext.SaveChanges();

                //var workingTime = dbContext.WorkingTime.FirstOrDefault(w => w.Id == 2);

                //workingTime.StartTime();

                //dbContext.SaveChanges();


                Console.WriteLine("End");
            }











        }

    }
}
