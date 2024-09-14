using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProjectClock.BusinessLogic.Services.RaportServices;
using ProjectClock.Database.Extensions;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace ProjectClock.MVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddRazorOptions(options =>
                 {
                     options.ViewLocationFormats.Add("/{0}.cshtml");
                 });

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddServices(builder.Configuration);


            builder.Services.AddScoped<IRaportServices, RaportServices>();



            builder.Services.AddDbContext<ProjectClock.Database.ProjectClockDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ProjectClock"));

            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.SlidingExpiration = true;
        options.LoginPath = "/Account/Login";

    });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("pl-PL")
                };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
            });

            var app = builder.Build();

            app.UseRequestLocalization();

            var scope = app.Services.CreateScope();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
