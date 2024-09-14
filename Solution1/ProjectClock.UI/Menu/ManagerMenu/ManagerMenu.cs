using ProjectClock.BusinessLogic.Models;
using ProjectClock.BusinessLogic.Services;
using ProjectClock.UI.Menu.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ProjectClock.UI.Menu.Manager
{
    internal class ManagerMenu
    {
        public int SelectedIndex { private set; get; }
        private MenuServices _menuServices = new MenuServices();
        
        public List<string> MainManagerMenuOptions { private set; get; } = new List<string> {
            "Create new project",
            "Remove project",
            "Display all projects",
            "Modify existing project",
            "Start working",
            "Stop working",
            "Exit"
        };

        internal void RunManagerMenu()
        {
            bool wantExit = false;                       

            do
            {

                string prompt = "  ";

                SelectedIndex = MenuServices.MoveableMenu(prompt, MainManagerMenuOptions, MainMenu.Intro());

                switch (SelectedIndex)
                {
                    case 0:

                        ManagerServicesProvider.CreateNewProject();
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey(true);
                        break;

                    case 1:

                        ManagerServicesProvider.RemoveProject();
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey(true);
                        break;                        

                    case 2:

                        ManagerServicesProvider.ShowAllProjects();
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey(true);
                        break;

                    case 3:

                        ManagerServicesProvider.ModifyProject();
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey(true);
                        break;

                    case 4:

                        ManagerServicesProvider.StartWorking();
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey(true);
                        break;
                    
                    case 5:

                        ManagerServicesProvider.StopWorking();
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey(true);
                        break;
                    
                    case 6:

                        ExitMenu.ExitFromProgramUsingAnyKey();                     
                        break;
                }

            } while (!wantExit);

        }

    }
}
