using BLL.Models;
using BLL.Services.IServices;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Blog.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("Home/ErrorProd/{statusCode}")]
        public IActionResult ErrorProd(int? statusCode = null)
        {
            _logger.LogError($"An error occurred. Redirect to {statusCode}");

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
            return View("ErrorProd", new ErrorViewModel { StatusCode = statusCode});
        }
    }
}