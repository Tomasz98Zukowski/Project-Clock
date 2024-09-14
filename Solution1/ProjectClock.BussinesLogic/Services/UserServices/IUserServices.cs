using ProjectClock.Database.Entities;

namespace ProjectClock.BusinessLogic.Services.UserServices
{
    public interface IUserServices
    {
        Task<int> Create(User user);
        Task<User> GetById(int id);
        Task<List<User>> GetAll();
        Task Update(User model);
        Task<bool> Delete(int id);
        Task<bool> UserExist(string email);
        Task<User?> GetByEmail(string email);

    }
}
