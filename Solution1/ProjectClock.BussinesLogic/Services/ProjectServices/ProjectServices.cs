using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectClock.BusinessLogic.Dtos.Project.Dtos;
using ProjectClock.BusinessLogic.Dtos.Project.ProjectDtos;
using ProjectClock.BusinessLogic.Services.EmailHostedServices;
using ProjectClock.Database;
using ProjectClock.Database.Entities;

namespace ProjectClock.BusinessLogic.Services.ProjectServices
{
    public class ProjectServices : IProjectServices
    {
        private ProjectClockDbContext _projectClockDbContext;
        private IMapper _mapper;
        

        public ProjectServices(ProjectClockDbContext projectClockDbContext, 
            IMapper mapper)
        {
            _projectClockDbContext = projectClockDbContext;
            _mapper = mapper;
        }

        public async Task<bool> Create(CreateProjectDto dto)
        {
            if (await _projectClockDbContext.Projects
            .AsNoTracking()
            .AnyAsync(p => p.Name == dto.ProjectName
                && p.Organization.Name == dto.OrganizationName))
            {
                return false;
            }

            var project = new Project()
            {
                Name = dto.ProjectName,
                Organization = await _projectClockDbContext.Organizations.SingleOrDefaultAsync(e => e.Name == dto.OrganizationName),
            };

            await _projectClockDbContext.Projects.AddAsync(project);
            await _projectClockDbContext.SaveChangesAsync();
            return true;
        }



        public async Task<Project> GetById(int id)
        {
            return await _projectClockDbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ProjectDto> GetProject(int projectId)
        {
            var entity = await _projectClockDbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            var dto = _mapper.Map<ProjectDto>(entity);

            return dto;
        }

        public async Task<IEnumerable<ProjectDto>> GetAll()
        {
            var list = await _projectClockDbContext.Projects.Include(p => p.Organization).ToListAsync();

            var dtos = _mapper.Map<IEnumerable<ProjectDto>>(list);

            return dtos;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllUserProjects(int userId)
        {
            var organizations = await _projectClockDbContext.OrganizationsUsers.Where(o => o.UserId == userId && o.AcceptedInvitation == true).ToListAsync();

            var list = new List<Project>();

            foreach(var org in organizations) 
            {                
                list = list.Concat
                (await _projectClockDbContext.Projects
                .Where(p => p.OrganizationId == org.OrganizationId)
                .Include(p => p.Organization)
                .ToListAsync())
                .ToList();
            }

            var dtos = _mapper.Map<IEnumerable<ProjectDto>>(list);

            return dtos;
        }

        public async Task Update(ProjectDto model)
        {
            var project = await GetById(model.Id);
            project.Name = model.Name;

            _projectClockDbContext.Projects.Update(project);
            await _projectClockDbContext.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var project = await GetById(id);

                if (project is null)
                {
                    throw new Exception($"Project with {id} doesn't exist");
                }
                else
                {
                    _projectClockDbContext.Projects.Remove(project);
                    await _projectClockDbContext.SaveChangesAsync();
                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<IEnumerable<ProjectWithAccessLevelDto>> GetAllUserProjectsFromOrganizationWhereIsOwnerOrManager(int userId)
        {
            var organizations = await _projectClockDbContext.OrganizationsUsers.Where(o => o.UserId == userId && (o.Role == Position.Owner || o.Role == Position.Manager) ).ToListAsync();

            var list = new List<Project>();

            foreach (var org in organizations)
            {
                list = list.Concat
                (await _projectClockDbContext.Projects
                .Where(p => p.OrganizationId == org.OrganizationId)
                .Include(p => p.Organization)
                .ToListAsync())
                .ToList();
            }

            var dtos = _mapper.Map<IEnumerable<ProjectWithAccessLevelDto>>(list);
            dtos.ToList().ForEach(dto => dto.CanEdit = true);

            return dtos;
        }

        public async Task<IEnumerable<ProjectWithAccessLevelDto>> GetAllUserProjectsFromOrganizationWhereIsUser(int userId)
        {
            var organizations = await _projectClockDbContext.OrganizationsUsers.Where(o => o.UserId == userId && o.Role == Position.User && o.AcceptedInvitation == true).ToListAsync();

            var list = new List<Project>();

            foreach (var org in organizations)
            {
                list = list.Concat
                (await _projectClockDbContext.Projects
                .Where(p => p.OrganizationId == org.OrganizationId)
                .Include(p => p.Organization)
                .ToListAsync())
                .ToList();
            }

            var dtos = _mapper.Map<IEnumerable<ProjectWithAccessLevelDto>>(list);

            return dtos;
        }
    }

}

