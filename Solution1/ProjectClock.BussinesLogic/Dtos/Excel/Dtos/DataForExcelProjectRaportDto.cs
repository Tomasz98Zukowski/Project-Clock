namespace ProjectClock.BusinessLogic.Dtos.Excel.Dtos;

public class DataForExcelProjectRaportDto
{
    public string ProjectName { get; set; } = string.Empty;
    public string OrganizationName { get; set; } = string.Empty;
    public string FromDate { get; set; } = string.Empty;
    public string ToDate { get; set; } = string.Empty;
    public string GenerateDate { get; set; } = string.Empty;
    public List<UserWithTimeDto> UserData { get; set; } = new List<UserWithTimeDto>();
}
