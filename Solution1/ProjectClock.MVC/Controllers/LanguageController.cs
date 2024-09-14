using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ProjectClock.MVC.Models;
using Microsoft.Extensions.Localization;

namespace ProjectClock.MVC.Controllers
{
    public class LanguageController : Controller
    {

        public IActionResult Index()
        {
            var model = new LanguageSettingsViewModel
            {
                AvailableLanguages = new List<string> { "en-US", "pl-PL" },
                SelectedLanguage = CultureInfo.CurrentCulture.Name
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return RedirectToAction("Index", "Home");
        }
    }
}
