using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ProjectClock.UI.Menu.Services
{
    internal class MenuServices
    {
        internal static void DisplayOptions(int selectedIndex, string prompt, List<string> options)
        {
            WriteLine(prompt);

            for (int i = 0; i < options.Count; i++)
            {
                string currentOption = options[i];
                string prefix;

                if (i == selectedIndex)
                {
                    prefix = "*";
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    prefix = " ";
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Black;
                }

                WriteLine($" {prefix} << {currentOption} >>");
            }

            ResetColor();

        }

        internal static int MoveableMenu(string prompt, List<string> options, string introMenu = "")
        {
            ConsoleKey keyPressed;
            int selectedIndex = 0;

            do
            {
                Clear();

                ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(introMenu);
                ResetColor();

                DisplayOptions(selectedIndex, prompt, options);

                ConsoleKeyInfo keyInfo = ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex == -1)
                    {
                        selectedIndex = options.Count - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex == options.Count)
                    {
                        selectedIndex = 0;
                    }
                }



            } while (keyPressed != ConsoleKey.Enter);

            return selectedIndex;
        }



    }
}
