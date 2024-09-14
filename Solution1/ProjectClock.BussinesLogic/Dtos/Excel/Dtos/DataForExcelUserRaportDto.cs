using ProjectClock.BusinessLogic.Dtos.Project.ProjectDtos;

namespace ProjectClock.BusinessLogic.Dtos.Excel.Dtos;

public class DataForExcelUserRaportDto
{
    public string UserName { get; set; } = string.Empty;
    public string UserSurname { get; set; } = string.Empty;
    public string FromDate { get; set; } = string.Empty;
    public string ToDate { get; set; } = string.Empty;
    public string GenerateDate { get; set; } = string.Empty;
    public List<ProjectWithTimeDto> ProjectData { get; set; } = new List<ProjectWithTimeDto>();
    public List<OrganizationWithTimeDto> OrganizationData { get; set; } = new List<OrganizationWithTimeDto>();
}
