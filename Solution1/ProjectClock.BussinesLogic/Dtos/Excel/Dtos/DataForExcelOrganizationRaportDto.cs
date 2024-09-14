namespace ProjectClock.BusinessLogic.Dtos.Excel.Dtos;

public class DataForExcelOrganizationRaportDto
{
    public string UserName { get; set; } = string.Empty;
    public string OrganizationName { get; set; } = string.Empty;
    public string FromDate { get; set; } = string.Empty;
    public string ToDate { get; set; } = string.Empty;
    public string GenerateDate { get; set; } = string.Empty;
    public List<ProjectWithTimeDto> OrganizationDataProjects { get; set; } = new List<ProjectWithTimeDto>();
    public List<UserWithTimeDto> OrganizationDataUsers { get; set; } = new List<UserWithTimeDto>();
}
