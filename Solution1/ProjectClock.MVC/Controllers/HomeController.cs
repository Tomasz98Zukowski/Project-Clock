using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectClock.BusinessLogic.Dtos.WorkingTime.WorkingTimeDtos;
using ProjectClock.BusinessLogic.Services.AccountServices;
using ProjectClock.BusinessLogic.Services.ProjectServices;
using ProjectClock.BusinessLogic.Services.WorkingTimeServices;
using ProjectClock.MVC.Extensions;
using ProjectClock.MVC.Models;
using System.Diagnostics;


namespace ProjectClock.MVC.Controllers
{
    public class HomeController : Controller
    {       
        private readonly IProjectServices _projectService;
        private readonly IAccountServices _accountService;
        private readonly IWorkingTimeServices _workingTimeServices;
        

        public HomeController(IProjectServices serviceProject,
            IWorkingTimeServices workingTimeServices, 
            IAccountServices accountService)
        {
            _projectService = serviceProject;            
            _accountService = accountService;
            _workingTimeServices = workingTimeServices;
        }

        [Authorize(Roles = "User")]
        public async Task <IActionResult> Index()
        {
            if (!HttpContext.User.Claims.TryGetAuthenticatedUserId(out var accountId))
            {
                return RedirectToAction("Index", "Home");
            }

            var userId = await _accountService.GetUserIdFromAccountId(accountId);
            var dtos = await _projectService.GetAllUserProjects(userId); 

            return View(dtos);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index(StartStopWorkingTimeDto dto)
        {
            if (!HttpContext.User.Claims.TryGetAuthenticatedUserId(out var accountId))
            {
                return RedirectToAction("Index", "Home");
            }

            dto.UserId = await _accountService.GetUserIdFromAccountId(accountId);

            var wt = await _workingTimeServices.Create(dto);

             if (wt)
                {
                    TempData["SuccessMessage"] = "Working time started";
                }
            else
                {
                    TempData["ErrorMessage"] = "You already work on this project.";
                }

            return RedirectToAction("Index", "Home");
        }
       




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
