namespace ProjectClock.BusinessLogic.Dtos.Project.Dtos;

public class ProjectWithAccessLevelDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Organization { get; set; }
    public bool CanEdit { get; set; } = false;
}
