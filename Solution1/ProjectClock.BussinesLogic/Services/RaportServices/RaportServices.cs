using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectClock.BusinessLogic.Dtos.AccountDtos;
using ProjectClock.BusinessLogic.Dtos.Raport;
using ProjectClock.BusinessLogic.Dtos.WorkingTime.WorkingTimeDtos;
using ProjectClock.BusinessLogic.Services.WorkingTimeServices;
using ProjectClock.Database;
using ProjectClock.Database.Entities;

using ProjectClock.BusinessLogic.Services.UserServices;

using ProjectClock.BusinessLogic.Services.ProjectServices;


namespace ProjectClock.BusinessLogic.Services.RaportServices
{

    public class RaportServices : IRaportServices
    {

        private ProjectClockDbContext _projectClockDbContext;
        private IMapper _mapper;     
		private readonly IUserServices _userService;
        private readonly IProjectServices _projectService;
        private readonly IWorkingTimeServices _workingTimeServices;
        private readonly ILogger<RaportServices> _logger;
        private readonly ProjectClockDbContext _dbContext;



       



        public RaportServices(ProjectClockDbContext projectClockDbContext, IMapper mapper, IUserServices userService, IProjectServices projectService, IWorkingTimeServices workingTimeServices, ILogger<RaportServices> logger, ProjectClockDbContext dbContext)
        {
            _projectClockDbContext = projectClockDbContext;
            _mapper = mapper;
			_userService = userService;
            _projectService = projectService;
            _workingTimeServices = workingTimeServices;
            _logger = logger;
            _dbContext = dbContext;

        }


        public async Task<bool> AddProjectToOrganization(int organizationId, Project project)
        {

            var organization = await _projectClockDbContext.Organizations.FindAsync(organizationId);

            if (organization == null)
            {
                return false;
            }


            organization.Projects.Add(project);
            await _projectClockDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<WorkingTime>> GetUserWorkingTimesAfterUserId(int userId)
        {

            var list = await _projectClockDbContext.WorkingTimes

                 .Where(e => e.UserId == userId)
                 .ToListAsync();


            return list;
        }



		public async Task<List<WorkingTime>> GetProjectsWorkingTimesAfterProjectId(int projectId)
		{

			var list = await _projectClockDbContext.WorkingTimes

				 .Where(e => e.ProjectId == projectId)
				 .ToListAsync();


			return list;
		}



		public async Task<List<Project>> GetAllProjects()
        {

            var list = await _projectClockDbContext.Projects.ToListAsync();


            return list;
        }

       
        public async Task<List<WorkingTime>> GetAll()
        {

            return await _projectClockDbContext.WorkingTimes.ToListAsync();

        }


		public async Task<IEnumerable<WorkingTimeDto>> GetUserAllWorkingTimes(int userId)
		{
			var list = await _projectClockDbContext.WorkingTimes


				.Where(e => e.UserId == userId)
				.Include(wt => wt.Project)
				.Include(wt => wt.User)
				.ToListAsync();

			var dtos = _mapper.Map<IEnumerable<WorkingTimeDto>>(list);

			return dtos;
		}
 

        public async Task<Model> GetProjectNameAndTimeForUser(int userId)
        {
            Model dto = new Model();

         
            dto.workingTimes = await GetUserWorkingTimesAfterUserId(userId);

            dto.ListUsersForRaports = await _userService.GetAll();

            dto.allProjects = await GetAllProjects();






            dto.projectsAfterUserId = dto.allProjects.Where(m => dto.workingTimes.Select(x => x.ProjectId).Contains(m.Id)).ToList();







            foreach (var item in dto.projectsAfterUserId)
            {


                var da = dto.workingTimes.Where(e => e.ProjectId == item.Id && e.EndTime != null).ToList();


                dto.totalTime_1 = da.Select(e => e.EndTime - e.StartTime).ToList();



                var totalTime = TimeSpan.Zero;
                foreach (TimeSpan currentValue in dto.totalTime_1)
                {
                    totalTime = totalTime + currentValue;
                }


                dto.totalTime_sum.Add(totalTime);

            }








            return dto;
        }


		public async Task<Model> GetDataForChart(int userId)
		{
			Model dto = new Model();

			
			dto.workingTimes = await GetUserWorkingTimesAfterUserId(userId);

			dto.ListUsersForRaports = await _userService.GetAll();

			dto.allProjects = await GetAllProjects();

			dto.projectsAfterUserId = dto.allProjects.Where(m => dto.workingTimes.Select(x => x.ProjectId).Contains(m.Id)).ToList();



			foreach (var item in dto.projectsAfterUserId)
			{


				var da = dto.workingTimes.Where(e => e.ProjectId == item.Id && e.EndTime != null).ToList();


				dto.totalTime_1 = da.Select(e => e.EndTime - e.StartTime).ToList();



				var totalTime = TimeSpan.Zero;
				foreach (TimeSpan currentValue in dto.totalTime_1)
				{
					totalTime = totalTime + currentValue;
				}





				dto.totalTime_sum.Add(totalTime);



			}

			return dto;
		}



		//================================================================ Project ===================================================================


		public async Task<Model> GetUserNameAndTimeForProject(int ProjectId)
		{
			Model dto = new Model();





			dto.workingTimes = await GetUserWorkingTimesAfterUserId(ProjectId);

			dto.ListUsersForRaports = await _userService.GetAll();

			dto.allProjects = await GetAllProjects();

			dto.allUsers = await _userService.GetAll();


			dto.projectsAfterUserId = dto.allProjects.Where(m => dto.workingTimes.Select(x => x.ProjectId).Contains(m.Id)).ToList();



			foreach (var item in dto.projectsAfterUserId)
			{


				var da = dto.workingTimes.Where(e => e.ProjectId == item.Id).ToList();


				dto.totalTime_1 = da.Select(e => e.EndTime - e.StartTime).ToList();



				var totalTime = TimeSpan.Zero;
				foreach (TimeSpan currentValue in dto.totalTime_1)
				{
					totalTime = totalTime + currentValue;
				}


				dto.totalTime_sum.Add(totalTime);

			}

			return dto;
		}



	}



   



}
