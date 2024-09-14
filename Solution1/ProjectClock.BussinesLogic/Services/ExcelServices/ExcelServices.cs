using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using ProjectClock.BusinessLogic.Dtos.Excel.Dtos;
using SpreadsheetLight;

namespace ProjectClock.BusinessLogic.Services.ExcelServices;

public class ExcelServices : IExcelServices
{
    public MemoryStream GenerateExcelForUser(string templatePath, DataForExcelUserRaportDto dto)
    {
        using (SLDocument raport = new SLDocument(templatePath))
        {
            raport.SetCellValue(2, 2, dto.UserName);
            raport.SetCellValue(2, 5, dto.UserSurname);
            raport.SetCellValue(3, 2, dto.FromDate);
            raport.SetCellValue(3, 3, dto.ToDate);
            raport.SetCellValue(3, 5, dto.GenerateDate);
            var i = 5;
            foreach(var project in dto.ProjectData)
            {
                raport.SetCellValue(i, 1, project.Name);
                raport.SetCellValue(i, 2, project.OrganizationName);
                raport.SetCellValue(i, 3, TotalTimeFormatToInt((int)project.TotalTime.TotalMinutes));
                
                raport.AutoFitColumn(1);
                raport.AutoFitColumn(2);
                raport.AutoFitColumn(3);
                raport.AutoFitColumn(4);
                raport.AutoFitColumn(5);

                SLStyle style = raport.CreateStyle();
                SLFont font = raport.CreateFont();
                
                font.SetFont("Calibri", 15);

                style.Font = font;
                style.Fill.SetPatternType(PatternValues.Solid);
                style.Fill.SetPatternForegroundColor(System.Drawing.Color.FromArgb(164, 194, 244));

                raport.SetCellStyle(i, 1, style);
                raport.SetCellStyle(i, 2, style);
                raport.SetCellStyle(i, 3, style);
                raport.SetCellStyle(i, 4, style);
                raport.SetCellStyle(i, 5, style);

                i++;
            }

            var outputStream = new MemoryStream();
            raport.SaveAs(outputStream);
            outputStream.Position = 0;

            return outputStream;
        }
    }
    public MemoryStream GenerateExcelForOrganization(string templatePath, DataForExcelOrganizationRaportDto dto)
    {
        using (SLDocument raport = new SLDocument(templatePath))
        {
            SLStyle style = raport.CreateStyle();
            SLFont font = raport.CreateFont();

            font.SetFont("Calibri", 15);

            style.Font = font;
            style.Fill.SetPatternType(PatternValues.Solid);
            style.Fill.SetPatternForegroundColor(System.Drawing.Color.FromArgb(164, 194, 244));

            SLStyle style2 = raport.CreateStyle();

            style2.Font = font;
            style2.Fill.SetPatternType(PatternValues.Solid);
            style2.Fill.SetPatternForegroundColor(System.Drawing.Color.FromArgb(109, 158, 235));

            raport.SetCellValue(2, 2, dto.OrganizationName);
            raport.SetCellValue(2, 5, dto.UserName);
            raport.SetCellValue(3, 2, dto.FromDate);
            raport.SetCellValue(3, 3, dto.ToDate);
            raport.SetCellValue(3, 5, dto.GenerateDate);
            var i = 5;
            foreach (var project in dto.OrganizationDataProjects)
            {
                raport.SetCellValue(i, 1, project.Name);
                raport.SetCellValue(i, 2, project.OrganizationName);
                raport.SetCellValue(i, 3, TotalTimeFormatToInt((int)project.TotalTime.TotalMinutes));

                raport.AutoFitColumn(1);
                raport.AutoFitColumn(2);
                raport.AutoFitColumn(3);
                raport.AutoFitColumn(4);
                raport.AutoFitColumn(5);

                

                raport.SetCellStyle(i, 1, style);
                raport.SetCellStyle(i, 2, style);
                raport.SetCellStyle(i, 3, style);
                raport.SetCellStyle(i, 4, style);
                raport.SetCellStyle(i, 5, style);

                i++;
            }

            raport.SetCellValue(i, 1, "Name");
            raport.SetCellValue(i, 2, "Surname");
            raport.SetCellValue(i, 3, "Total time");

            raport.SetCellStyle(i, 1, style2);
            raport.SetCellStyle(i, 2, style2);
            raport.SetCellStyle(i, 3, style2);
            raport.SetCellStyle(i, 4, style2);
            raport.SetCellStyle(i, 5, style2);

            i++;

            foreach (var user in dto.OrganizationDataUsers)
            {

                raport.SetCellValue(i, 1, user.Name);
                raport.SetCellValue(i, 2, user.Surname);
                raport.SetCellValue(i, 3, TotalTimeFormatToInt((int)user.TotalTime.TotalMinutes));

                raport.AutoFitColumn(1);
                raport.AutoFitColumn(2);
                raport.AutoFitColumn(3);
                raport.AutoFitColumn(4);
                raport.AutoFitColumn(5);



                raport.SetCellStyle(i, 1, style);
                raport.SetCellStyle(i, 2, style);
                raport.SetCellStyle(i, 3, style);
                raport.SetCellStyle(i, 4, style);
                raport.SetCellStyle(i, 5, style);

                i++;
            }

            var outputStream = new MemoryStream();
            raport.SaveAs(outputStream);
            outputStream.Position = 0;

            return outputStream;
        }
    }


    public MemoryStream GenerateExcelForProject(string templatePath, DataForExcelProjectRaportDto dto)
    {
        using (SLDocument raport = new SLDocument(templatePath))
        {
            raport.SetCellValue(2, 2, dto.ProjectName);
            raport.SetCellValue(2, 5, dto.OrganizationName);
            raport.SetCellValue(3, 2, dto.FromDate);
            raport.SetCellValue(3, 3, dto.ToDate);
            raport.SetCellValue(3, 5, dto.GenerateDate);
            var i = 5;
            foreach (var user in dto.UserData)
            {
                raport.SetCellValue(i, 1, user.Name);
                raport.SetCellValue(i, 2, user.Surname);
                raport.SetCellValue(i, 3, TotalTimeFormatToInt((int)user.TotalTime.TotalMinutes));

                raport.AutoFitColumn(1);
                raport.AutoFitColumn(2);
                raport.AutoFitColumn(3);
                raport.AutoFitColumn(4);
                raport.AutoFitColumn(5);

                SLStyle style = raport.CreateStyle();
                SLFont font = raport.CreateFont();

                font.SetFont("Calibri", 15);

                style.Font = font;
                style.Fill.SetPatternType(PatternValues.Solid);
                style.Fill.SetPatternForegroundColor(System.Drawing.Color.FromArgb(164, 194, 244));

                raport.SetCellStyle(i, 1, style);
                raport.SetCellStyle(i, 2, style);
                raport.SetCellStyle(i, 3, style);
                raport.SetCellStyle(i, 4, style);
                raport.SetCellStyle(i, 5, style);

                i++;
            }

            var outputStream = new MemoryStream();
            raport.SaveAs(outputStream);
            outputStream.Position = 0;

            return outputStream;
        }
    }
    private static int TotalTimeFormatToInt(int time)
    {
        var upOrDown = time % 60;
        int totalTime;
        if (upOrDown < 29)
        {
            totalTime = time / 60;
        }
        else
        {
            totalTime = (time / 60) + 1;
        }

        return totalTime;
    }
}
