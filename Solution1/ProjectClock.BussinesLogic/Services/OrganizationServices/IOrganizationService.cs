using ProjectClock.BusinessLogic.Dtos.Organization;
using ProjectClock.Database.Entities;

namespace ProjectClock.BusinessLogic.Services.OrganizationServices
{
    public interface IOrganizationService
    {
        Task<bool> Create(CreateOrganizationDto organization);
        Task<Organization> GetById(int id);
        Task<List<Organization>> GetAll();
        Task<List<OrganizationDto>> GetAllUserOrganization(int userId);
        Task Update(Organization model);
        Task<bool> Delete(int id);
        Task<bool> OrganizationExist(string name);
        Task<bool> AddUser(int organizationId, int userId);
        Task<List<OrganizationDto>> GetAllUserOrganizationWhereIsManagerOrOwner(int userId);
    }
}
