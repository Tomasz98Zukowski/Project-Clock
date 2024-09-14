namespace ProjectClock.BusinessLogic.Dtos.Excel.Dtos
{
    public class ProjectWithTimeDto
    {
        public string Name { get; set; } = string.Empty;
        public string OrganizationName { get; set; } = string.Empty;
        public TimeSpan TotalTime { get; set; } = TimeSpan.Zero;
        public string Color { get; set; } = string.Empty;
    }
}