using Microsoft.EntityFrameworkCore;
using ProjectClock.BusinessLogic.Dtos.Project.ProjectDtos;
using ProjectClock.Database;
using ProjectClock.Database.Entities;

namespace ProjectClock.BusinessLogic.Services.UserServices
{
    public class UserServices : IUserServices
    {
        private ProjectClockDbContext _projectClockDbContext;
        public UserServices(ProjectClockDbContext projectClockDbContext)
        {
            _projectClockDbContext = projectClockDbContext;
        }

        public async Task<int> Create(User user)
        {
            if (await UserExist(user.Email))
            {
                throw new Exception($"This user already exist"); //czy tu tresc exception ma sens skoro nie bedzie wystwietlana?

            }
            else
            {
                _projectClockDbContext.Users.Add(user);
                await _projectClockDbContext.SaveChangesAsync();
                return user.Id;
            }
        }

        public async Task<User?> GetById(int id)
        {
            return await _projectClockDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _projectClockDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<User>> GetAll()
        {
            return await _projectClockDbContext.Users.ToListAsync();
        }

        public async Task Update(User model)
        {
            var user = await GetById(model.Id);

            user.Name = model.Name;
            user.Email = model.Email;
            user.Surname = model.Surname;

            await _projectClockDbContext.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var user = await GetById(id);

                if (user is null)
                {
                    throw new Exception($"This user doesn't exist");
                    return false;
                }
                else
                {

                    _projectClockDbContext.Users.Remove(user);
                    await _projectClockDbContext.SaveChangesAsync();
                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<bool> UserExist(string email)
        {
            return await _projectClockDbContext.Users.AsNoTracking().AnyAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllFromOrganization(int organizationId)
        {
            var users = await _projectClockDbContext.OrganizationsUsers.Where(o => o.OrganizationId == organizationId)
                .Select(u => u.User).ToListAsync();
            return users;
        }

        public async Task<bool> SignUserToOrganization(User user, Organization organization)
        {
            try
            {
                OrganizationUser organizationUser = new OrganizationUser() { User = user, Organization = organization };
                await _projectClockDbContext.OrganizationsUsers.AddAsync(organizationUser);
                await _projectClockDbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
