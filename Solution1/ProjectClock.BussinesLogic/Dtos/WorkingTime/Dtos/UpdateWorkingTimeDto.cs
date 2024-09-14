namespace ProjectClock.BusinessLogic.Dtos.WorkingTime.WorkingTimeDtos
{
    public class UpdateWorkingTimeDto
    {
        public int Id { get; set; } 
        public DateTime StartTime { get; set; }
        public DateTime EndTime {get; set;}
        public string? Description {get; set;}
    }
}