using ProjectClock.UI.Menu.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ProjectClock.UI.Menu
{
    internal class ExitMenu
    {
        internal static void ExitFromProgramUsingAnyKey()
        {
            WriteLine("\nPress any key to exit...");
            ReadKey(true);
            Environment.Exit(0);
        }

        internal static void ExitByPressingEscToManagerMenu()
        {
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                ManagerMenu managerMenu = new ManagerMenu();
                managerMenu.RunManagerMenu();
                return;
            }
        }

        internal static void PressAnyKeyToContinue()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }      

    }
}
