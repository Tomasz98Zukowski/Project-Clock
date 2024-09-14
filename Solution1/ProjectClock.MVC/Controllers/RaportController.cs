using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectClock.BusinessLogic.Dtos.Raport;
using ProjectClock.BusinessLogic.Services.ProjectServices;
using ProjectClock.BusinessLogic.Services.RaportServices;
using ProjectClock.BusinessLogic.Services.UserServices;
using ProjectClock.BusinessLogic.Services.WorkingTimeServices;
using ProjectClock.Database;


namespace ProjectClock.MVC.Controllers
{
    public class RaportController : Controller
    {

        private readonly IUserServices _userService;
        private readonly IProjectServices _projectService;
        private readonly IWorkingTimeServices _workingTimeServices;
        private readonly IRaportServices _raportServices;
        private readonly ILogger<RaportController> _logger;

        private readonly ProjectClockDbContext _dbContext;



        public RaportController(IUserServices userService, IProjectServices projectService, IWorkingTimeServices workingTimeServices, ILogger<RaportController> logger, IRaportServices raportService, ProjectClockDbContext dbContext)
        {
            _userService = userService;
            _projectService = projectService;
            _workingTimeServices = workingTimeServices;
            _logger = logger;
            _raportServices = raportService;
            _dbContext = dbContext;
        }







        //======================================================= User ==============================================================


        [Authorize(Roles = "User")]
        public async Task<IActionResult> User()
        {

            Model dto = new Model();

            var ListAllUsers = await _userService.GetAll();
            //int zm = ListAllUsers.FirstOrDefault(a => a.Id > 0).Id;
            int zm = ListAllUsers.FirstOrDefault().Id;
            TempData["Id"] = zm.ToString();








            dto = await _raportServices.GetProjectNameAndTimeForUser(zm);





            return View(dto);

        }



        [HttpPost]
        public async Task<IActionResult> User(int userId)
        {

            try
            {
                var dto = await _raportServices.GetProjectNameAndTimeForUser(userId);

                TempData["Id"] = userId.ToString();

                return View("User", dto);

            }
            catch (Exception ex)
            {

                return View();
            }

        }


        [Authorize(Roles = "User")]
        //[HttpGet]
        public async Task<IActionResult> ChartOfUser()
        {

            int zm = int.Parse(TempData["Id"].ToString());

            var dto = await _raportServices.GetDataForChart(zm);

            return View(dto);

        }



        //======================================================= Project ==============================================================



        [Authorize(Roles = "User")]
        public async Task<IActionResult> Project()
        {


            Model dto = new Model();


            dto.ListProjectsForRaports = await _raportServices.GetAllProjects();


            int zm = dto.ListProjectsForRaports.FirstOrDefault(a => a.Id > 0).Id;


            //dto = await _raportServices.GetUserNameAndTimeForProject(zm);

            //dto = await _raportServices.get(zm);




            TempData["IdProjChart"] = zm.ToString();


            return View(dto);

        }


        // POST: OrganizationController/Create
        [HttpPost]
        //	[ValidateAntiForgeryToken]
        public async Task<IActionResult> Project(int projectId)
        {

            try
            {
                var dto = await _raportServices.GetProjectNameAndTimeForUser(projectId);

                TempData["IdProjChart"] = projectId.ToString();

                return View("Project", dto);

            }
            catch (Exception ex)
            {

                return View();
            }

        }



        [Authorize(Roles = "User")]
        //[HttpGet]
        public async Task<IActionResult> ChartOfProject()
        {

            int zm_2 = int.Parse(TempData["IdProjChart"].ToString());
            //int zm_2 = 1;
            var dto = await _raportServices.GetDataForChart(zm_2);

            return View(dto);

        }







        //======================================================= Organization ==============================================================



        [Authorize(Roles = "User")]
        public IActionResult Organization()
        {
            return View();
        }







    }
}
