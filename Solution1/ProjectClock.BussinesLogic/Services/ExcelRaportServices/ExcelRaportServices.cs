using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using ProjectClock.BusinessLogic.Dtos.Excel;
using ProjectClock.BusinessLogic.Dtos.Excel.Dtos;
using ProjectClock.BusinessLogic.Services.OrganizationServices;
using ProjectClock.BusinessLogic.Services.ProjectServices;
using ProjectClock.BusinessLogic.Services.UserServices;
using ProjectClock.BusinessLogic.Services.WorkingTimeServices;
using ProjectClock.Database;
using ProjectClock.Database.Entities;

namespace ProjectClock.BusinessLogic.Services.ExcelRaportServices;

public partial class ExcelRaportServices : IExcelRaportServices
{
    private readonly IProjectServices _projectServices;
    private readonly ProjectClockDbContext _projectClockDbContext;


    public ExcelRaportServices(IProjectServices organizationServices,
        ProjectClockDbContext projectClockDbContext)
    {
        _projectServices = organizationServices;
        _projectClockDbContext = projectClockDbContext;
    }

    public async Task<DataForExcelUserRaportDto> GenerateDataUser(GenerateDataDto dto)
    {
        var userProjects = await _projectServices.GetAllUserProjects(dto.userId);
        var user = _projectClockDbContext.Users.AsNoTracking().FirstOrDefault(u => u.Id == dto.userId);
        var data = new DataForExcelUserRaportDto()
        {
            UserName = user.Name,
            UserSurname = user.Surname,
            FromDate = dto.fromDate.ToString("dd-MM-yyyy"),
            ToDate = dto.toDate.ToString("dd-MM-yyyy"),
            GenerateDate = DateTime.Now.ToString("dd-MM-yyyy")
        };
        foreach (var project in userProjects)
        {
            var raportData = new ProjectWithTimeDto()
            {
                Name = project.Name ?? "-",
                OrganizationName = project.Organization ?? "-",
            };
            var worktimes = _projectClockDbContext.WorkingTimes
                .AsNoTracking()
                .Where(wt => wt.UserId == dto.userId
                        && wt.EndTime != null
                        && wt.ProjectId == project.Id
                        && wt.StartTime.Value.Date >= dto.fromDate.Date
                        && dto.toDate.Date >= wt.StartTime.Value.Date)
                .ToList();

            if(worktimes.Any())
            {
                foreach (var worktime in worktimes)
                {
                    var workTimeTotal = worktime.EndTime - worktime.StartTime;
                    raportData.TotalTime += (TimeSpan)workTimeTotal;
                }
            }
            
            data.ProjectData.Add(raportData);
        }

        var organizationList = data.ProjectData.Select(pd => pd.OrganizationName).ToList().Distinct();
        foreach (var organization in organizationList)
        {
            var raportData = new OrganizationWithTimeDto()
            {
                Name = organization
            };

            var workingtimes = data.ProjectData.Where(pd => pd.OrganizationName == organization).Select(pd => pd.TotalTime).ToList();
            foreach(var workingtime in workingtimes)
            {
                raportData.TotalTime += workingtime;
            }

            data.OrganizationData.Add(raportData);
        }

        return data;
    }
    public async Task<DataForExcelProjectRaportDto> GenerateDataProject(GenerateDataDto dto)
    {

        var project = _projectClockDbContext.Projects
            .AsNoTracking()
            .Include(p => p.Organization)
            .FirstOrDefault(p => p.Id == dto.projectId);

        var usersFromOrganization = await _projectClockDbContext.OrganizationsUsers
            .AsNoTracking()
            .Where(o => o.OrganizationId == project.OrganizationId)
            .Select(u => u.UserId)
            .ToListAsync();

        var data = new DataForExcelProjectRaportDto()
        {
            ProjectName = project.Name ?? "-",
            OrganizationName = project.Organization.Name ?? "-",
            FromDate = dto.fromDate.ToString("dd-MM-yyyy"),
            ToDate = dto.toDate.ToString("dd-MM-yyyy"),
            GenerateDate = DateTime.Now.ToString("dd-MM-yyyy")
        };
        foreach (var userId in usersFromOrganization)
        {
            var user = _projectClockDbContext.Users.FirstOrDefault(u => u.Id == userId);

            var raportData = new UserWithTimeDto()
            {
                Name = user.Name,
                Surname = user.Surname
            };
            var worktimes = _projectClockDbContext.WorkingTimes
                .AsNoTracking()
                .Where(wt => wt.UserId == user.Id
                        && wt.EndTime != null
                        && wt.ProjectId == project.Id
                        && wt.StartTime.Value.Date >= dto.fromDate.Date
                        && dto.toDate.Date >= wt.StartTime.Value.Date)
                .ToList();
            if (worktimes.Any())
            {
                foreach (var worktime in worktimes)
                {
                    var workTimeTotal = worktime.EndTime - worktime.StartTime;
                    raportData.TotalTime += (TimeSpan)workTimeTotal;
                }
            }
            
            data.UserData.Add(raportData);
        }
        return data;
    }
    public async Task<DataForExcelOrganizationRaportDto> GenerateDataOrganization(GenerateDataDto dto)
    {
        var organization = _projectClockDbContext.Organizations
           .AsNoTracking()
           .Include(p => p.Projects)
           .FirstOrDefault(p => p.Id == dto.organizationId);

        var user = _projectClockDbContext.Users
           .AsNoTracking()
           .FirstOrDefault(p => p.Id == dto.userId);

        var data = new DataForExcelOrganizationRaportDto()
        {
            OrganizationName = organization.Name ?? "-",
            UserName = $"{user.Name} {user.Surname}",
            FromDate = dto.fromDate.ToString("dd-MM-yyyy"),
            ToDate = dto.toDate.ToString("dd-MM-yyyy"),
            GenerateDate = DateTime.Now.ToString("dd-MM-yyyy")
        };
        foreach (var project in organization.Projects)
        {

            var raportData = new ProjectWithTimeDto()
            {
                Name = project.Name,
                OrganizationName = organization.Name
            };
            var worktimes = _projectClockDbContext.WorkingTimes
                .AsNoTracking()
                .Where(wt => 
                           wt.EndTime != null
                        && wt.ProjectId == project.Id
                        && wt.StartTime.Value.Date >= dto.fromDate.Date
                        && dto.toDate.Date >= wt.StartTime.Value.Date)
                .ToList();

            if (worktimes.Any())
            {
                foreach (var worktime in worktimes)
                {
                    var workTimeTotal = worktime.EndTime - worktime.StartTime;
                    raportData.TotalTime += (TimeSpan)workTimeTotal;
                }
            }
                data.OrganizationDataProjects.Add(raportData);

            
        }

        var users = _projectClockDbContext.OrganizationsUsers
            .AsNoTracking()
            .Include(o => o.User)
            .Where(o => o.OrganizationId == organization.Id)
            .Select(o => o.User)
            .ToList();

        foreach (var organizationUser in users)
        {

            var raportData = new UserWithTimeDto()
            {
                Name = organizationUser.Name,
                Surname = organizationUser.Surname,
            };


            var worktimes = _projectClockDbContext.WorkingTimes
                .AsNoTracking()
                .Where(wt =>
                           wt.EndTime != null
                        && wt.UserId == organizationUser.Id
                        && organization.Projects.Select(p => p.Id).Contains(wt.ProjectId)
                        && wt.StartTime.Value.Date >= dto.fromDate.Date
                        && dto.toDate.Date >= wt.StartTime.Value.Date)
                .ToList();

            if (worktimes.Any())
            {
                foreach (var worktime in worktimes)
                {
                    var workTimeTotal = worktime.EndTime - worktime.StartTime;
                    raportData.TotalTime += (TimeSpan)workTimeTotal;
                }
            }

            
            data.OrganizationDataUsers.Add(raportData);
        }

        return data;
    }
}
