using ProjectClock.BusinessLogic.Dtos.Raport;
using ProjectClock.BusinessLogic.Dtos.WorkingTime.WorkingTimeDtos;
using ProjectClock.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectClock.BusinessLogic.Services.RaportServices
{
	public interface IRaportServices
	{
		

		Task<bool> AddProjectToOrganization(int organizationId, Project project);

		Task<List<WorkingTime>> GetAll();

		Task<List<WorkingTime>> GetUserWorkingTimesAfterUserId(int userId);

		Task<List<Project>> GetAllProjects();

		Task<IEnumerable<WorkingTimeDto>> GetUserAllWorkingTimes(int userId);

		Task<Model> GetProjectNameAndTimeForUser(int userId);

		Task<Model> GetDataForChart(int userId);



		//=========================================== Project =====================================================


		Task<Model> GetUserNameAndTimeForProject(int userId);


		Task<List<WorkingTime>> GetProjectsWorkingTimesAfterProjectId(int projectId);


	}
}
