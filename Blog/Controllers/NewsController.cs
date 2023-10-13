using BLL.Models.News;
using BLL.Services.IServices;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly UserManager<User> _userManager;
        public NewsController(INewsService newsService, UserManager<User> userManager)
        {
            _newsService = newsService;
            _userManager = userManager;
        }

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpGet]
        [Route("News/Add")]
        public async Task<IActionResult> AddNews()
        {
            var model = _newsService.AddNews();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [Route("News/Add")]
        public async Task<IActionResult> AddNews(AddNewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var news = await _newsService.AddNews(model);
                if (news)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return RedirectToAction("AddNews");
        }

        [AuthorizationEditPost]
        [HttpGet]
        [Route("News/Edit/{id}")]
        public async Task<IActionResult> EditNews(string id)
        {
            var model = await _newsService.EditNews(id);
            if (model == null)
                return StatusCode(404);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [Route("News/Edit/{id}")]
        public async Task<IActionResult> EditNews(EditNewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _newsService.EditNews(model);
                if (result)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return View("EditPost", model);
        }

        [HttpPost]
        [AuthorizationEditPost]
        [Route("News/RemoveNews/{id}")]
        public async Task<IActionResult> RemoveNews(string id)
        {
            await _newsService.RemoveNews(id);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("News/AllNews")]
        public async Task<IActionResult> AllNews()
        {
            var news = await _newsService.AllNews();

            return View(new AllNewsViewModel { News = news });
        }
    }
}
