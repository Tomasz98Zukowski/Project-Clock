using ProjectClock.BusinessLogic.Dtos.Excel.Dtos;

namespace ProjectClock.BusinessLogic.Services.ExcelRaportServices
{
    public interface IExcelRaportServices
    {
        Task<DataForExcelOrganizationRaportDto> GenerateDataOrganization(GenerateDataDto dto);
        Task<DataForExcelProjectRaportDto> GenerateDataProject(GenerateDataDto dto);
        Task<DataForExcelUserRaportDto> GenerateDataUser(GenerateDataDto dto);
    }
}