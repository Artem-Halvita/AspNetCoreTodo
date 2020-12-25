using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;
using AspNetCoreToDo;

namespace AspNetCoreTodo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;

            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            //var alreadyExists = await _roleManager.RoleExistsAsync(Constants.AdministratorRole);

            //if (alreadyExists) return View();

            //await _roleManager.CreateAsync(new IdentityRole(Constants.AdministratorRole));

            //var user = await _userManager.FindByEmailAsync("artem@gmail.com");

            //await _userManager.AddToRoleAsync(user, Constants.AdministratorRole);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
