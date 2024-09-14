namespace ProjectClock.BusinessLogic.Dtos.Excel.Dtos
{
    public class OrganizationWithTimeDto
    {
        public string Name { get; set; } = string.Empty;
        public TimeSpan TotalTime { get; set; } = TimeSpan.Zero;
    }
}