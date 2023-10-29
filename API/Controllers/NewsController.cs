using BLL.Models.News;
using BLL.Services.IServices;
using Blog;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<NewsController> _logger;
        public NewsController(INewsService newsService, UserManager<User> userManager, ILogger<NewsController> logger)
        {
            _newsService = newsService;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        [Route("API/News/Add")]
        public async Task<IActionResult> AddNews([FromBody] AddNewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var news = await _newsService.AddNews(model);
                if (news != null)
                {
                    _logger.LogInformation($"the user added new news {news}");
                    return StatusCode(200);
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return StatusCode(400);
        }

        [Authorize]
        [HttpPatch]
        [Route("API/News/Edit")]
        public async Task<IActionResult> EditNews([FromBody] EditNewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _newsService.EditNews(model);
                if (result)
                {
                    _logger.LogInformation($"the user edited news {model.Id}");
                    return StatusCode(200);
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            return StatusCode(404);
        }

        [HttpDelete]
        [AuthorizationEditPost]
        [Route("API/News/RemoveNews/{id}")]
        public async Task<IActionResult> RemoveNews([FromRoute] string id)
        {
            var result = await _newsService.RemoveNews(id);
            if (result)
            {
                _logger.LogWarning($"the user deleted news {id}");
                return StatusCode(200);
            }
            else
            {
                return StatusCode(404);
            }
        }

        [HttpGet]
        [Route("API/News/AllNews")]
        public async Task<List<News>> AllNews()
        {
            var news = await _newsService.AllNews();

            return news;
        }
    }
}
