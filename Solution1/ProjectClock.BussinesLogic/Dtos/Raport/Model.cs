using ProjectClock.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjectClock.BusinessLogic.Dtos.Raport
{
    public class Model
    {
        public int Id { get; set; }

        public List<Database.Entities.User>? ListUsersForRaports { get; set; } = new List<Database.Entities.User>();

		public List<Database.Entities.User>? allUsers { get; set; } = new List<Database.Entities.User>();

		public List<Database.Entities.Project>? ListProjectsForRaports { get; set; } = new List<Database.Entities.Project>();


		public List<Database.Entities.WorkingTime>? workingTimes { get; set; } = new List<Database.Entities.WorkingTime>();

        public List<Database.Entities.Project>? allProjects { get; set; } = new List<Database.Entities.Project>();

        public List<Database.Entities.Project>? projectsAfterUserId { get; set; } = new List<Database.Entities.Project>();

		public List<TimeSpan?> totalTime_1 { get; set; }  = new  List<TimeSpan?>();

        public List<TimeSpan?> totalTime_sum { get; set; } = new List<TimeSpan?>();

        public bool Chart_on_off { get; set; } = false;


	}
}

    

