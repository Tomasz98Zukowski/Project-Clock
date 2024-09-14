using ProjectClock.UI.Menu.Manager;
using ProjectClock.UI.Menu.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectClock.UI.Menu.EmployeeMenu;

internal class UserMenu
{
    public int SelectedIndex { private set; get; }
    private MenuServices _menuServices = new MenuServices();

    public List<string> MainUserMenuOptions { private set; get; } = new List<string> {           
        "Display all projects",         
        "Start working",
        "Stop working",
        "Exit"
    };

    internal void RunUserMenu()
    {
        bool wantExit = false;

        do
        {

            string prompt = "  ";

            SelectedIndex = MenuServices.MoveableMenu(prompt, MainUserMenuOptions, MainMenu.Intro());

            switch (SelectedIndex)
            {
                case 0:

                    ManagerServicesProvider.ShowAllProjects();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey(true);
                    break;

                case 1:

                    ManagerServicesProvider.StartWorking();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey(true);
                    break;

                case 2:

                    ManagerServicesProvider.StopWorking();
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey(true);
                    break;

                case 3:

                    ExitMenu.ExitFromProgramUsingAnyKey();
                    break;       
            }

        } while (!wantExit);

    }
}
