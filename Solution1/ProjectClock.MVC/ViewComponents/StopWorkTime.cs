using Microsoft.AspNetCore.Mvc;
using ProjectClock.BusinessLogic.Services.AccountServices;
using ProjectClock.BusinessLogic.Services.WorkingTimeServices;
using ProjectClock.MVC.Extensions;
namespace ProjectClock.MVC.Services.Components;

public class StopWorkTime : ViewComponent
{
    private readonly IWorkingTimeServices _workingTimeServices;
    private readonly IAccountServices _accountService;

    public StopWorkTime(IWorkingTimeServices workingTimeServices
        , IAccountServices accountService)
    {
        _workingTimeServices = workingTimeServices;
        _accountService = accountService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        HttpContext.User.Claims.TryGetAuthenticatedUserId(out var accountId);

        var userId = await _accountService.GetUserIdFromAccountId(accountId);
        var dto = await _workingTimeServices.GetUserNotFinisedWorkingTimes(userId);

        return View(dto);
    }
}
