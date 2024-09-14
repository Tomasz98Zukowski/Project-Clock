using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using ProjectClock.BusinessLogic.Models;
using ProjectClock.UI.Menu.Services;
using ProjectClock.UI.Menu.Manager;
using static System.Console;
using ProjectClock.BusinessLogic.Services.UserServices;
using System.Drawing;
using System.Runtime.CompilerServices;
using ProjectClock.UI.Menu.EmployeeMenu;

namespace ProjectClock.UI.Menu
{
    public class MainMenu
    {       
        public static User User { private set; get; }
        private string prompt;
        private string[] options;

        public static string Intro()
        {
            StringBuilder sb = new StringBuilder();

            

            string intro = @"
   ___              _              _      ___  _               _    
  / _ \ _ __  ___  (_)  ___   ___ | |_   / __\| |  ___    ___ | | __
 / /_)/| '__|/ _ \ | | / _ \ / __|| __| / /   | | / _ \  / __|| |/ /
/ ___/ | |  | (_) || ||  __/| (__ | |_ / /___ | || (_) || (__ |   < 
\/     |_|   \___/_/ | \___| \___| \__|\____/ |_| \___/  \___||_|\_\
                 |__/ 
";


            sb.Append(intro);

            string introTextFirstLine = ("       ProjectClock allows to manage your employees worktime");
            string introTextSecondLine = ("    Use up and down to navigate and press the Enter key to select");
            

            sb.AppendLine(introTextFirstLine);
            sb.AppendLine(introTextSecondLine);
           


            return sb.ToString();

        }


        public static void RunMenu()
        {
            bool isLogged = false;
            User = new User();          


            do
            {
                Console.Clear();
                ForegroundColor = ConsoleColor.Cyan;
                AlignTextInOneThirdOfScreen(Intro());
                ResetColor();

                string questionName = "Enter your name:";
                AlignTextInOneThirdOfScreen(questionName);
                string name = Console.ReadLine();
                
                string questionSurname = "Enter your surname:";
                AlignTextInOneThirdOfScreen(questionSurname);
                string surname = Console.ReadLine();



                if (AccessService.IsLogged(name, surname))
                {
                    User.Name = name;
                    User.Surname = surname;
                    User.Id = General.GetUserIdByNameAndSurname(name, surname);
                    Position? userPosition = General.GetUserPosition(name, surname);

                    if ( userPosition == Position.Manager)
                    {
                        string answear = $"      \t   Welcome {name} {surname}. You're logged in as a {userPosition}!";
                        ForegroundColor = ConsoleColor.Green;
                        AlignTextInOneThirdOfScreen(answear);
                        ResetColor();                     

                        string pressAnyKey = "Press any key to continue...";
                        AlignTextInOneThirdOfScreen(pressAnyKey);
                       
                        Console.ReadKey(true);

                        ManagerMenu managerMenu = new ManagerMenu();
                        managerMenu.RunManagerMenu();                      
                    }

                    if (userPosition == Position.User)
                    {
                        string answear = $"      \t   Welcome {name} {surname}. You're logged in as a {userPosition}!";
                        ForegroundColor = ConsoleColor.Green;
                        AlignTextInOneThirdOfScreen(answear);
                        ResetColor();

                        string pressAnyKey = "Press any key to continue...";
                        AlignTextInOneThirdOfScreen(pressAnyKey);

                        Console.ReadKey(true);

                        UserMenu userMenu = new UserMenu();
                        userMenu.RunUserMenu();
                    }

                    return;
                }

                Console.WriteLine();
                string failAnswear = "           You entered name and surname that is not in our system.";
                ForegroundColor = ConsoleColor.Red;
                AlignTextInOneThirdOfScreen(failAnswear);
                ResetColor();
                
                string tryAgain = "     Try one more time. Press Escape to exit or any other key to continue...";
                AlignTextInOneThirdOfScreen(tryAgain);
                Console.WriteLine();

                if (ReadKey(true).Key == ConsoleKey.Escape) 
                {                   
                    Environment.Exit(0);
                    //return;
                }                

            } while (!isLogged);          

        }

        private static void AlignTextInOneThirdOfScreen(string text)
        {
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 3) + (text.Length / 3)) + "}", text));
        }
    }
}
