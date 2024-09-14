using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectClock.BusinessLogic.Services.AccountServices;
using ProjectClock.BusinessLogic.Services.ExcelRaportServices;
using ProjectClock.BusinessLogic.Services.ExcelServices;
using ProjectClock.BusinessLogic.Services.OrganizationServices;
using ProjectClock.BusinessLogic.Services.ProjectServices;
using ProjectClock.Database.Entities;
using ProjectClock.MVC.Extensions;

namespace ProjectClock.MVC.Controllers;


public class AwesomeReportController : Controller
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IExcelRaportServices _excelRaportServices;
    private readonly IAccountServices _accountServices;
    private readonly IExcelServices _excelServices;
    private readonly IProjectServices _projectServices;
    private readonly IOrganizationService _organizationServices;

    public AwesomeReportController(IWebHostEnvironment hostingEnvironment, 
        IExcelRaportServices excelRaportServices, 
        IAccountServices accountServices, 
        IExcelServices excelServices, 
        IProjectServices projectServices, 
        IOrganizationService organizationServices)
    {
        _hostingEnvironment = hostingEnvironment;
        _excelRaportServices = excelRaportServices;
        _accountServices = accountServices;
        _excelServices = excelServices;
        _projectServices = projectServices;
        _organizationServices = organizationServices;
    }

    [Authorize(Roles = "User")]
    public async Task<IActionResult> UserReport()
    {
        if (!HttpContext.User.Claims.TryGetAuthenticatedUserId(out var accountId))
        {
            return RedirectToAction("Index", "Home");
        }

        var userId = await _accountServices.GetUserIdFromAccountId(accountId);

        var dto = new GenerateDataDto()
        {
            userId = userId,
            fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
            toDate = DateTime.Now,
            projects = await _projectServices.GetAllUserProjectsFromOrganizationWhereIsOwnerOrManager(userId),
            organizations = await _organizationServices.GetAllUserOrganizationWhereIsManagerOrOwner(userId)
        };

        var data = await _excelRaportServices.GenerateDataUser(dto);

        dto.userData = data;


        return View(dto);
    }
    [HttpPost]
    public async Task<IActionResult> GenerateUserAwesomeReport(GenerateDataDto dto)
    {
        if (!HttpContext.User.Claims.TryGetAuthenticatedUserId(out var accountId))
        {
            return RedirectToAction("Index", "Home");
        }

        dto.userId = await _accountServices.GetUserIdFromAccountId(accountId);

        var data = await _excelRaportServices.GenerateDataUser(dto);

        dto.userData = data;


        return View("UserReport", dto);
    }

    [Authorize(Roles = "User")]
    public async Task<IActionResult> ProjectReport()
    {
        if (!HttpContext.User.Claims.TryGetAuthenticatedUserId(out var accountId))
        {
            return RedirectToAction("Index", "Home");
        }

        var userId = await _accountServices.GetUserIdFromAccountId(accountId);

        var dto = new GenerateDataDto()
        {
            userId = userId,
            fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
            toDate = DateTime.Now,
            projects = await _projectServices.GetAllUserProjectsFromOrganizationWhereIsOwnerOrManager(userId),
            organizations = await _organizationServices.GetAllUserOrganizationWhereIsManagerOrOwner(userId)
        };

        dto.projectId = dto.projects.FirstOrDefault().Id;

        if(dto.projectId != null)
        {
            var data = await _excelRaportServices.GenerateDataProject(dto);
            dto.projectData = data;
        }

        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> GenerateProjectAwesomeReport(GenerateDataDto dto)
    {
        if (!HttpContext.User.Claims.TryGetAuthenticatedUserId(out var accountId))
        {
            return RedirectToAction("Index", "Home");
        }

        dto.userId = await _accountServices.GetUserIdFromAccountId(accountId);
        dto.projects = await _projectServices.GetAllUserProjectsFromOrganizationWhereIsOwnerOrManager(dto.userId);
        dto.organizations = await _organizationServices.GetAllUserOrganizationWhereIsManagerOrOwner(dto.userId);

        var data = await _excelRaportServices.GenerateDataProject(dto);

        dto.projectData = data;


        return View("ProjectReport", dto);
    }

    [Authorize(Roles = "User")]
    public async Task<IActionResult> OrganizationReport()
    {
        if (!HttpContext.User.Claims.TryGetAuthenticatedUserId(out var accountId))
        {
            return RedirectToAction("Index", "Home");
        }

        var userId = await _accountServices.GetUserIdFromAccountId(accountId);

        var dto = new GenerateDataDto()
        {
            userId = userId,
            fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
            toDate = DateTime.Now,
            projects = await _projectServices.GetAllUserProjectsFromOrganizationWhereIsOwnerOrManager(userId),
            organizations = await _organizationServices.GetAllUserOrganizationWhereIsManagerOrOwner(userId)
        };

        dto.organizationId = dto.organizations.FirstOrDefault().OrganizationId;

        if (dto.organizationId != null)
        {
            var data = await _excelRaportServices.GenerateDataOrganization(dto);
            dto.organizationData = data;
        }

        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> GenerateOrganizationAwesomeReport(GenerateDataDto dto)
    {
        if (!HttpContext.User.Claims.TryGetAuthenticatedUserId(out var accountId))
        {
            return RedirectToAction("Index", "Home");
        }

        dto.userId = await _accountServices.GetUserIdFromAccountId(accountId);
        dto.projects = await _projectServices.GetAllUserProjectsFromOrganizationWhereIsOwnerOrManager(dto.userId);
        dto.organizations = await _organizationServices.GetAllUserOrganizationWhereIsManagerOrOwner(dto.userId);

        var data = await _excelRaportServices.GenerateDataOrganization(dto);
        dto.organizationData = data;


        return View("OrganizationReport", dto);
    }
}
