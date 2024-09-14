namespace ProjectClock.BusinessLogic.Dtos.Excel.Dtos
{
    public class UserWithTimeDto
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public TimeSpan TotalTime { get; set; } = TimeSpan.Zero;
        public string Color { get; set; } = string.Empty;
    }
}