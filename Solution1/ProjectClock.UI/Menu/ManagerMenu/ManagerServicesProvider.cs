using ProjectClock.BusinessLogic.Models;
using ProjectClock.BusinessLogic.Services;
using ProjectClock.BusinessLogic.Services.EntryTimeServices;
using ProjectClock.BusinessLogic.Services.ProjectServices;
using ProjectClock.BusinessLogic.Services.WorkingTimeRecorder;
using ProjectClock.UI.Menu.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectClock.UI.Menu.Manager
{
    internal class ManagerServicesProvider
    {
        private static int _userId = MainMenu.User.Id;
        public static int SelectedIndex { private set; get; }
        public static int _idOfChoosenProject { private set; get; }
        public static Project _selectedProject { private set; get; }
        public static List<Project> _projects { get; private set; } = ProjectGetter.GetProjectList();
        public static List<StartWork> _openProjectsByUserId { get; private set; } = WorkingTimeRecorder.AllProjectsOpenedByUser(_userId);

        internal static void CreateNewProject()
        {

            Console.WriteLine("\nInsert the name of new project:");

            string projectName = Console.ReadLine();

            ProjectCreator.CreateProject(projectName);

            _projects = ProjectGetter.GetProjectList();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nProject {projectName} has been created.");
            Console.ResetColor();

        }

        internal static void ShowAllProjects()
        {
            Console.Clear();
            Console.WriteLine(MainMenu.Intro());

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nList of projects:\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            foreach (var project in _projects)
            {
                Console.WriteLine($" Project Id: {project.Id,-5}    Name: {project.Name,-30}");
            }
            Console.ResetColor();
        }

        internal static void RemoveProject()
        {
            Console.WriteLine("\nInsert Id of a project you want to remove:");

            if (int.TryParse(Console.ReadLine(), out int id) && ProjectRemover.RemoveProject(id))
            {
                Console.WriteLine($"Project with Id {id} has been removed.");
                _projects = ProjectGetter.GetProjectList();

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You entered either id that doesn't exit");
                Console.ResetColor();            
            }

        }

        internal static void ModifyProject()
        {
            int oldId;
            int newId;

            ShowAllProjects();

            string oldIdQuestion = "Insert id of a project you want to modify:";

            Console.WriteLine();
            Console.WriteLine(oldIdQuestion);

            while (!(int.TryParse(Console.ReadLine(), out oldId) && General.IdVerificator(oldId)))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You entered id that doesn't exist. Press Escape to exit or any other key to continue.");
                Console.ResetColor();

                ExitMenu.ExitByPressingEscToManagerMenu();

                Console.WriteLine();
                Console.WriteLine(oldIdQuestion);
            }


            string newIdQuestion = $"Insert new id in place of old one {oldId}:";

            Console.WriteLine();
            Console.WriteLine(newIdQuestion);

            while (!int.TryParse(Console.ReadLine(), out newId) || (General.IdVerificator(newId)))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You entered either non-integer id or id that exist. Press Escape to exit or any other key to conitue.");
                Console.ResetColor();

                ExitMenu.ExitByPressingEscToManagerMenu();

                Console.WriteLine();
                Console.WriteLine(newIdQuestion);
            }

            string newNameQuestion = "\nInsert new project name:";
            Console.WriteLine(newNameQuestion);
            string? newProjectName = Console.ReadLine();

            if (ProjectEditor.ModifyProject(oldId, newId, newProjectName))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Project id and/or name has been changed.");
                Console.ResetColor();
            }

            _projects = ProjectGetter.GetProjectList();

        }

        internal static void StopWorking()
        {         
            var listOfProjects = _projects;

            var joinedProjectsById = _openProjectsByUserId.Join(listOfProjects,
                p => p.ProjectID,
                proj => proj.Id,
                (id, projectName) => new { id.ProjectID, projectName.Name });

            var namesOfOpenProjects = joinedProjectsById.Select(p => p.Name).ToList();

            if (namesOfOpenProjects.Count == 0)
            {
                Console.WriteLine("\nYou are not currently working on any project.");
                return;
            }

            string prompt = $"List of open projects by user {MainMenu.User.Name} {MainMenu.User.Surname}:\n";
            int selectedProjectFromOpenList = MenuServices.MoveableMenu(prompt, namesOfOpenProjects, MainMenu.Intro());
            
            int idOfChoosenToCloseProject = joinedProjectsById.ElementAt(selectedProjectFromOpenList).ProjectID;
            Project? project = _projects.FirstOrDefault(p => p.Id == idOfChoosenToCloseProject);
            
            Console.WriteLine($"\nSelected project with id: {project.Id}, name: {project.Name} is about to close. Push \'enter\' to confirm or other key to cancel.");

            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                WorkingTimeRecorder.StopWork(_userId, idOfChoosenToCloseProject);
                _openProjectsByUserId = WorkingTimeRecorder.AllProjectsOpenedByUser(_userId);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nProject closed.");
                Console.ResetColor();
            }            
        }

        internal static void StartWorking()
        {
            string prompt = String.Format("Choose project you want to work on:\n");

            var projectNames = _projects.Select(p => p.Name).ToList();

            SelectedIndex = MenuServices.MoveableMenu(prompt, projectNames, MainMenu.Intro());

            _idOfChoosenProject = _projects.ElementAt(SelectedIndex).Id;
            _selectedProject = _projects.ElementAt(SelectedIndex);
            int userId = MainMenu.User.Id;

            if (WorkingTimeRecorder.StartWork(userId, _idOfChoosenProject)) 
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nYour work began at \"{_selectedProject.Name}\".");
                Console.ResetColor();

                _openProjectsByUserId = WorkingTimeRecorder.AllProjectsOpenedByUser(_userId);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"\nProject with ID {_idOfChoosenProject} for user with ID: {userId} is in progres. Choose another one. \n");
                Console.ResetColor();
            }

        }
    }
}
