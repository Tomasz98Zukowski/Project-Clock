using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using static ProjectClock.BusinessLogic.Services.ExcelRaportServices.ExcelRaportServices;
using ProjectClock.BusinessLogic.Services.ExcelRaportServices;
using ProjectClock.BusinessLogic.Services.ExcelServices;
using ProjectClock.MVC.Extensions;
using ProjectClock.BusinessLogic.Dtos.Organization;
using ProjectClock.BusinessLogic.Services.UserServices;
using ProjectClock.BusinessLogic.Services.AccountServices;
using ProjectClock.BusinessLogic.Dtos.Excel.Dtos;
using ProjectClock.BusinessLogic.Services.ProjectServices;
using ProjectClock.BusinessLogic.Services.OrganizationServices;

namespace ProjectClock.MVC.Controllers
{
    public class ExcelController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IExcelRaportServices _excelRaportServices;
        private readonly IAccountServices _accountServices;
        private readonly IExcelServices _excelServices;
        private readonly IProjectServices _projectServices;
        private readonly IOrganizationService _organizationServices;
        public ExcelController(IWebHostEnvironment hostingEnvironment,
            IExcelRaportServices excelRaportServices,
            IExcelServices excelServices,
            IAccountServices accountServices,
            IProjectServices projectServices,
            IOrganizationService organizationServices)
        {
            _hostingEnvironment = hostingEnvironment;
            _excelRaportServices = excelRaportServices;
            _excelServices = excelServices;
            _accountServices = accountServices;
            _projectServices = projectServices;
            _organizationServices = organizationServices;
        }
        public async Task<IActionResult> Index()
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


            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateUserExcel(GenerateDataDto dto)
        {
            if (!HttpContext.User.Claims.TryGetAuthenticatedUserId(out var accountId))
            {
                return RedirectToAction("Index", "Home");
            }

            string templatePath = Path.Combine(_hostingEnvironment.WebRootPath, "excel_templates", "template_user.xlsx");
            dto.userId = await _accountServices.GetUserIdFromAccountId(accountId);

            var data = await _excelRaportServices.GenerateDataUser(dto);
            var fileStream = _excelServices.GenerateExcelForUser(templatePath, data);

            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"userraport_{dto.fromDate.ToString("y-MM-dd")}_{dto.userId}.xlsx");
        }

        [HttpPost]
        public async Task<IActionResult> GenerateProjectExcel(GenerateDataDto dto)
        {

            string templatePath = Path.Combine(_hostingEnvironment.WebRootPath, "excel_templates", "template_project.xlsx");

            var data = await _excelRaportServices.GenerateDataProject(dto);
            var fileStream = _excelServices.GenerateExcelForProject(templatePath, data);

            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                $"projectraport_{dto.fromDate.ToString("y-MM-dd")}_{dto.projectId}.xlsx");
        }

        [HttpPost]
        public async Task<IActionResult> GenerateOrganizationExcel(GenerateDataDto dto)
        {
            if (!HttpContext.User.Claims.TryGetAuthenticatedUserId(out var accountId))
            {
                return RedirectToAction("Index", "Home");
            }

            string templatePath = Path.Combine(_hostingEnvironment.WebRootPath, "excel_templates", "template_organization.xlsx");
            dto.userId = await _accountServices.GetUserIdFromAccountId(accountId);

            var data = await _excelRaportServices.GenerateDataOrganization(dto);
            var fileStream = _excelServices.GenerateExcelForOrganization(templatePath, data);

            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"organizationraport_{dto.fromDate.ToString("y-MM-dd")}_{dto.organizationId}.xlsx");
        }
    }
}
