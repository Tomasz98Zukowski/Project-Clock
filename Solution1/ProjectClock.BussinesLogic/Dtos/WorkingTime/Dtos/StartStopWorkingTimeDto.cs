using ProjectClock.BusinessLogic.Dtos.Project.ProjectDtos;
using ProjectClock.Database.Entities;

namespace ProjectClock.BusinessLogic.Dtos.WorkingTime.WorkingTimeDtos;

public class StartStopWorkingTimeDto
{
    public int WorkingTimeId { get; set; }
    public int UserId { get; set; }
    public string? ProjectName { get; set; }
    public string? Description { get; set; }
    public IEnumerable<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
}
