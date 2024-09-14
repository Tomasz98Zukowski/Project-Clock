using ProjectClock.BusinessLogic.Dtos.WorkingTime.WorkingTimeDtos;
using ProjectClock.Database.Entities;

namespace ProjectClock.BusinessLogic.Services.WorkingTimeServices
{
    public interface IWorkingTimeServices
    {
        Task<bool> Create(StartStopWorkingTimeDto dto);
        Task<bool> Update(UpdateWorkingTimeDto dto);
        Task<WorkingTimeDto>? GetById(int id);
        Task<List<WorkingTime>> GetAll();
        Task<bool> Delete(int id);
        bool WorkingTimeExist(StartStopWorkingTimeDto dto);
        Task<IEnumerable<WorkingTimeDto>> GetUserNotFinisedWorkingTimes(int userId);
        Task<IEnumerable<WorkingTimeDto>> GetUserAllWorkingTimes(int userId);
        Task<bool> StopWork(StartStopWorkingTimeDto dto);
        
    }

}

