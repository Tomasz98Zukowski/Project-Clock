using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectClock.BusinessLogic.Dtos.WorkingTime.WorkingTimeDtos;
using ProjectClock.BusinessLogic.Services.AccountServices;
using ProjectClock.BusinessLogic.Services.WorkingTimeServices;
using ProjectClock.MVC.Extensions;

namespace ProjectClock.MVC.Controllers
{
    public class WorkingTimeController : Controller
    {
        private readonly IWorkingTimeServices _workingTimeServices;
        private readonly IAccountServices _accountService;

        public WorkingTimeController(IWorkingTimeServices workingTimeServices,
            IAccountServices accountService)
        {
            _workingTimeServices = workingTimeServices;
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            HttpContext.User.Claims.TryGetAuthenticatedUserId(out var accountId);

            
            var userId = await _accountService.GetUserIdFromAccountId(accountId);
            var dto = await _workingTimeServices.GetUserAllWorkingTimes(userId);
            return View(dto);
        }


        [Route("WorkingTime/Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var dto = await _workingTimeServices.GetById(id);

            return View(dto);
        }


        [HttpPost]
        [Route("WorkingTime/Update/{id}")]
        public async Task<IActionResult> Update(UpdateWorkingTimeDto dto, int id)
        {
            if (!ModelState.IsValid)
            {
                var dto2 = await _workingTimeServices.GetById(id);
                return View(dto2);
            }
            dto.Id = id;
            await _workingTimeServices.Update(dto);

            return RedirectToAction("Index", "WorkingTime");
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Stop(StartStopWorkingTimeDto dto)
        {
            if (!HttpContext.User.Claims.TryGetAuthenticatedUserId(out var accountId))
            {
                return RedirectToAction("Index", "Home");
            }

            await _workingTimeServices.StopWork(dto);

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("WorkingTime/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _workingTimeServices.Delete(id);

            return RedirectToAction("Index", "WorkingTime");
        }

        public async Task<ActionResult> GetTime(int selectedProjectId)
        {
            HttpContext.User.Claims.TryGetAuthenticatedUserId(out var accountId);

            string data;
            var userId = await _accountService.GetUserIdFromAccountId(accountId);
            var dtos = await _workingTimeServices.GetUserNotFinisedWorkingTimes(userId);

            if (dtos != null)
            {
                var projectStartTime = dtos.Where(wt => wt.Id == selectedProjectId).Select(wt => wt.StartTime).SingleOrDefault();
                var time = DateTime.UtcNow - projectStartTime;
                data = time.ToString(@"hh\:mm\:ss");
            }
            else
            {
                data = DateTime.Now.ToString(@"hh\:mm\:ss");
            }
            

            return Content(data);
        }
    }
}
