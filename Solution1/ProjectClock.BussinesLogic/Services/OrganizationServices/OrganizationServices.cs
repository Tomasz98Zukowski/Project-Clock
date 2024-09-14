using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectClock.BusinessLogic.Dtos.Organization;
using ProjectClock.BusinessLogic.Dtos.Project.ProjectDtos;
using ProjectClock.Database;
using ProjectClock.Database.Entities;

namespace ProjectClock.BusinessLogic.Services.OrganizationServices
{

    public class OrganizationServices : IOrganizationService
    {
        private ProjectClockDbContext _projectClockDbContext;
        private IMapper _mapper;

        public OrganizationServices(ProjectClockDbContext projectClockDbContext, IMapper mapper)
        {
            _mapper = mapper;
            _projectClockDbContext = projectClockDbContext;
        }

        public OrganizationServices(ProjectClockDbContext projectClockDbContext)
        {
            _projectClockDbContext = projectClockDbContext;
        }
        public async Task<bool> Create(Organization organization)
        {
            try
            {
                if (await OrganizationExist(organization.Name))
                {
                    return false;
                }
                else
                {
                    await _projectClockDbContext.Organizations.AddAsync(organization);
                    await _projectClockDbContext.SaveChangesAsync();
                    return true;

                }

            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> Create(CreateOrganizationDto organizationDto)
        {
            var organization = new Organization()
            {
                Name = organizationDto.Name
            };

            var owner = new OrganizationUser()
            {
                User = _projectClockDbContext.Users.SingleOrDefault(e => e.Id == organizationDto.UserId),
                Organization = organization,
                Role = Position.Owner,
                AcceptedInvitation = true
            };




            try
            {
                if (await OrganizationExist(organization.Name))
                {
                    return false;
                }
                else
                {
                    await _projectClockDbContext.Organizations.AddAsync(organization);
                    await _projectClockDbContext.OrganizationsUsers.AddAsync(owner);
                    await _projectClockDbContext.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<Organization> GetById(int id)
        {
            return await _projectClockDbContext.Organizations.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<Organization>> GetAll()
        {
            return await _projectClockDbContext.Organizations.Include(o => o.Projects).Include(o => o.OrganizationUsers).ThenInclude(u => u.User).ToListAsync();
        }

        public async Task Update(Organization model)
        {
            var organization = await GetById(model.Id);

            organization.Name = model.Name;

            await _projectClockDbContext.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var organization = await GetById(id);

                if (organization is null)
                {
                    return false;
                }
                else
                {

                    _projectClockDbContext.Organizations.Remove(organization);
                    await _projectClockDbContext.SaveChangesAsync();
                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<bool> OrganizationExist(string name)
        {
            return await _projectClockDbContext.Organizations.AsNoTracking().AnyAsync(u => u.Name == name);
        }

        public async Task<bool> AddUser(int organizationId, int userId)
        {
            try
            {
                var user = await _projectClockDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                var organization = await GetById(organizationId);

                if (user is null || organization is null)
                {
                    return false;
                }
                else
                {
                    OrganizationUser organizationUser = new OrganizationUser() { User = user, Organization = organization, Role = Position.User, AcceptedInvitation = false};
                    await _projectClockDbContext.OrganizationsUsers.AddAsync(organizationUser);
                    await _projectClockDbContext.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
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

        public async Task<List<OrganizationDto>> GetAllUserOrganization(int userId)
        {

            var list = await _projectClockDbContext.Organizations
                .Where(o => o.OrganizationUsers
                .FirstOrDefault(e => e.UserId == userId).User.Id == userId)
                .ToListAsync();

            var dtos = _mapper.Map<List<OrganizationDto>>(list);

            return dtos;
        }

        public async Task<List<OrganizationDto>> GetAllUserOrganizationWhereIsManagerOrOwner(int userId)
        {
            var list = await _projectClockDbContext.Organizations
                .Where(o => o.OrganizationUsers.FirstOrDefault(e => e.UserId == userId).User.Id == userId
                    && o.OrganizationUsers.FirstOrDefault(e => e.UserId == userId).Role == Position.Owner 
                    || o.OrganizationUsers.FirstOrDefault(e => e.UserId == userId).Role == Position.Manager)
                .ToListAsync();

            var dtos = _mapper.Map<List<OrganizationDto>>(list);

            return dtos;
        }

        public async Task<bool> AcceptInvitation(int organizationId, int userId)
        {
            try
            {
                var ou = _projectClockDbContext.OrganizationsUsers.FirstOrDefault(ou =>
                    ou.OrganizationId == organizationId && ou.UserId == userId);

                if (ou is null)
                {
                    return false;
                }
                else
                {
                    ou.AcceptedInvitation = true;
                    _projectClockDbContext.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }


    }
}
