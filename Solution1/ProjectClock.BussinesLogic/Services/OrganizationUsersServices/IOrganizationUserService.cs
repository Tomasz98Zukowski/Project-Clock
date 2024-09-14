using ProjectClock.Database.Entities;

namespace ProjectClock.BusinessLogic.Services.OrganizationUserServices;

public interface IOrganizationUserService
{
    bool IsUserAnOwner(int userId);
    Task<IEnumerable<Organization>> GetUserOrganizations(int userId);
    Task<IEnumerable<User>> GetOrganizationUsers(int organizationId);
    Task<IEnumerable<User>> GetOrganizationUsersExceptOwner(int organizationId, int userId);
    Task<bool> IsUserSignedToOrganization(int userId, int organizationId);
    Task<IEnumerable<Organization>> GetUserAsAOwnerOrganization(int userId);
    Task<bool> RemoveUserFromOrganization(int userId, int organizationId);
    Task<bool> AdvanceUserToManager(int userId, int organizationId);
    Task<bool> IsUserAnOwnerOfParticularOrganization(int userId, int organizationId);
    Task<bool> DegradeManager(int userId, int organizationId);
    Task<IEnumerable<Organization>> GetInvitingOrganizations(int userId);
    Task<bool> AcceptInvitation(int userId, int organizationId);
}