using BLL.Models;
using BLL.Services.IServices;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            await _userService.GenerateData();
            return RedirectToAction("AllNews", "News");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Home/Error/{statusCode}")]
        public IActionResult Error(int? statusCode = null)
        {

            switch (statusCode)
            {
                case 404:
                    break;
                case 403:
                    break;
                default:
                    statusCode = 500;
                    break;
            }
            this.HttpContext.Response.StatusCode = (int)statusCode;

            return View("Error", new ErrorViewModel { StatusCode = statusCode});
        }
    }
}